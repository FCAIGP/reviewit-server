using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewItServer.Data;
using ReviewItServer.DTOs;
using ReviewItServer.Models;
using ReviewItServer.Utility;
using ReviewItServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReviewItServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReplyController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns Reply with specific id
        /// </summary>
        /// <param name="id">The id of the reply</param>
        /// <response code="404">Returned if no Reply found</response>
        /// <response code="200">Returned if Reply was feteched successfully</response>
        // GET: api/Reply/id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ReplyView>> GetReply(string id)
        {
            var reply = await _context.Replies.FindAsync(id);
            if(reply == null)
            {
                return NotFound();
            }
            return _mapper.Map<ReplyView>(reply);
        }

        /// <summary>
        /// Adds a new Reply to the system (requires User priveleges)
        /// </summary>
        /// <param name="dto">The data of the Reply</param>
        /// <response code="401">Returned if User is not Authorized (not logged in)</response>
        /// <response code="400">Returned if Authentication error occurs OR ParentID(Review ID) doesn't exist</response>
        /// <response code="201">Returned new Reply is created and successfully added to the system</response>
        // POST: api/Reply
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ReplyView>> CreateReply(ReplyDTO dto)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest("An authentication error has been encountered!");
            }
            var parent = await _context.Reviews.FindAsync(dto.ParentId);
            if(parent == null)
            {
                return BadRequest("Review referred to in parentId field doesn't exist.");
            }
            var reply = _mapper.Map<Reply>(dto);
            reply.Parent = parent;
            reply.Author = user;

            await _context.Replies.AddAsync(reply);
            parent.Replies.Add(reply);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReply", new { id = reply.ReplyId }, _mapper.Map<ReplyView>(reply));
        }

        /// <summary>
        /// Delete a Reply with specific id (requires User privileges)
        /// </summary>
        /// <param name="id">The id of the Reply to be deleted</param>
        /// <response code="401">Returned if User is not Authorized (not logged in)</response>
        /// <response code="400">Returned if Authentication error occurs</response>
        /// <response code="404">Returned if Reply was not found</response>
        /// <response code="204">Returned if deletion is handled successfully</response>
        // DELETE: api/Reply/id
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteReply(string id)
        {
            bool isAdmin = User.FindAll(ClaimTypes.Role).Any(v => v.Value == Roles.Admin);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest("An authentication error has been encountered!");
            }

            var reply = await _context.Replies.FindAsync(id);
            if (reply == null)
            {
                return NotFound();
            }
            if (isAdmin || userId == reply.AuthorId)
            {
                _context.Replies.Remove(reply);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest("You are not authorized to remove this reply.");
            }
        }
    }
}
