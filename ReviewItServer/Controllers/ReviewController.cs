using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewItServer.Data;
using ReviewItServer.DTOs;
using ReviewItServer.Models;
using ReviewItServer.ViewModels;

namespace ReviewItServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ReviewController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a Review with specific id
        /// </summary>
        /// <param name="id">The id of the Review</param>
        /// <response code="404">Returned if Review was not found</response>
        /// <response code="200">Returned if Review was fetched successfully</response>
        // GET: api/Review/id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ReviewView>> GetReview(string id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return _mapper.Map<ReviewView>(review);
        }

        /// <summary>
        /// Creates a new Review and adds it to the system
        /// </summary>
        /// <param name="dto">The data of the Review</param>
        /// <response code="400">Returned if Authenticated error occurs OR CompanyId doesn't exist</response>
        /// <response code="201">Returned if Review was created successfully and added to the system</response>
        // POST: api/Review
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ReviewView>> CreateReview(ReviewDTO dto)
        {
            User user = null;
            if (!dto.IsAnonymous)
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return BadRequest("Non-authenticated user is attempting to write a non-anonymous review, either check your authentication token or set IsAnonymous to true");
                }
            }
            var review = new Review();
            var company = await _context.Companies.FindAsync(dto.CompanyId);
            if(company == null)
            {
                return BadRequest("Company referred to in companyId field doesn't exist.");
            }
            _mapper.Map(dto, review);
            review.AuthorIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            review.Author = user;
            review.Company = company;

            await _context.Reviews.AddAsync(review);
            company.Reviews.Add(review);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReview", new { id = review.ReviewId }, _mapper.Map<ReviewView>(review));
        }

        /// <summary>
        /// Deletes a Review with specific id (requires Admin priveleges)
        /// </summary>
        /// <param name="id">The id of the Review to be deleted</param>
        /// <response code="401">Returned if User is not Authorized (not admin)</response>
        /// <response code="404">Returned if Review was not found</response>
        /// <response code="204">Returned if Review was deleted successfully</response>
        // DELETE: api/Review/id
        [Authorize(Roles = Utility.Roles.Admin)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteReview(string id)
        {

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns a list of Replies for a Review with specific id
        /// </summary>
        /// <param name="id">The id of the Review</param>
        /// <response code ="404">Returned if Review was not found</response>
        /// <response code ="200">Returned if list of Replies is fetched successfully</response>
        [HttpGet("{id}/replies")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReplyView>>> GetReplies(string id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            await _context.Entry(review).Collection(v => v.Replies).LoadAsync();
            return review.Replies.Select(v => _mapper.Map<ReplyView>(v)).ToList();
        }
    }
}
