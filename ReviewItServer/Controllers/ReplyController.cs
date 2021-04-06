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

        // GET: api/Reply/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ReplyView>> GetReply(string id)
        {
            var reply = await _context.Replies.FindAsync(id);
            if(reply == null)
            {
                return NotFound();
            }
            return _mapper.Map<ReplyView>(reply);
        }

        // POST: api/Reply
        [HttpPost]
        [Authorize]
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

        // DELETE: api/Reply/id
        [Authorize]
        [HttpDelete("{id}")]
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
