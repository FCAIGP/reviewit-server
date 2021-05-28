using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewItServer.Data;
using ReviewItServer.Models;
using ReviewItServer.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReviewItServer.Controllers
{
    [ApiController]
    [Route("api/Review")]
    public class VotesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Review/id/votes
        /// <summary>
        /// Returns Votes of a Review with specific id
        /// </summary>
        /// <param name="id">The id of the Review</param>
        /// <response code="404">Returned if Review was not found</response>
        /// <response code="200">Returned if Votes was fetched successfully</response>
        [HttpGet("{id}/votes")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VotesView>> GetVotes(string id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            await _context.Entry(review).Collection(v => v.Votes).LoadAsync();
            int allVotes = review.Votes.Count;
            int upvotes = review.Votes.Count(v => v.IsUpVote);
            VotesView votesView = new VotesView(upvotes, allVotes - upvotes);
            votesView.ReviewId = review.ReviewId;
            return votesView;
        }

        // PUT: api/Review/id/upvote
        /// <summary>
        /// Upvotes a Review with specific id
        /// </summary>
        /// <param name="id">The id of the Review</param>
        /// <response code="401">Returned if user is not Authorized (not logged in)</response>
        /// <response code = "404">Returned if Review was not found</response>
        /// <response code = "200">Returned if Review was Upvoted successfully OR Vote changed to Upvote</response>
        /// <response code = "400">Returned if Authenticaiton error occured</response>
        [HttpPut("{id}/upvote")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Upvote(string id)
        {
            return await PerformVote(id, true);
        }

        // PUT: api/Review/id/downvote
        /// <summary>
        /// Downvotes a Review with specific id
        /// </summary>
        /// <param name="id">The id of the Review</param>
        /// <response code = "404">Returned if Review was not found</response>
        /// <response code="401">Returned if user is not Authorized (not logged in)</response>
        /// <response code = "200">Returned if Review was Downvoted successfully OR Vote changed to Downvote</response>
        /// <response code = "400">Returned if Authenticaiton error occured</response>
        [HttpPut("{id}/downvote")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Downvote(string id)
        {
            return await PerformVote(id, false);
        }

        [NonAction]
        private async Task<ActionResult> PerformVote(string id, bool isUpVote)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            var ret = _context.Entry(review).Collection(v => v.Votes).Query().Where(v => v.ReviewId == id && v.UserId == userId).FirstOrDefault();
            if (ret != null)
            {
                if (ret.IsUpVote == isUpVote)
                {
                    return Ok(new { action = "none", message = $"Already {(isUpVote ? "up" : "down")}voted." });
                }
                else
                {
                    ret.IsUpVote = isUpVote;
                    await _context.SaveChangesAsync();
                    return Ok(new { action = "changed", message = $"Vote changed to an {(isUpVote ? "up" : "down")}vote." });
                }
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest("Bad authentication, check your authentication token.");
            }

            Vote vote = new Vote
            {
                Review = review,
                ReviewId = review.ReviewId,
                IsUpVote = isUpVote,
                UserId = user.Id,
                User = user
            };

            _context.Votes.Add(vote);
            review.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return Ok(new { action = "vote", message = $"Sucessfully {(isUpVote ? "up" : "down")}voted." });
        }
    }
}
