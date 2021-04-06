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

        // GET: api/Review/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewView>> GetReview(string id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return _mapper.Map<ReviewView>(review);
        }

        // POST: api/Review
        [HttpPost]
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

        // DELETE: api/Review/id
        [Authorize(Roles = Utility.Roles.Admin)]
        [HttpDelete("{id}")]
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

        [HttpGet("{id}/replies")]
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
