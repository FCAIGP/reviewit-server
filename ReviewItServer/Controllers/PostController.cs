using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PostController : ControllerBase
    {
      
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PostController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PostView>> GetPost(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if(post == null)
            {
                return NotFound();
            }
            return _mapper.Map<PostView>(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostView>> CreatePost(PostDTO dto)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest("An authentication error has been encountered!");
            }
            var company = await _context.Companies.FindAsync(dto.CompanyId);
            if(company.OwnerId != id)
            {
                return BadRequest("You are not the owner of this company page.");
            }
            var post = _mapper.Map<Post>(dto);
            post.Author = user;
            post.CreatedDate = DateTime.Now;
            post.Company = company;
            company.Posts.Add(post);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPost", new { id = post.PostId }, _mapper.Map<PostView>(post));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePost(string id)
        {
            string ID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(ID);
            if(user == null)
            {
                return BadRequest("Authentication token doesn't correspond to a valid user.");
            }

            var post = await _context.Posts.FindAsync(id);
            if(post == null)
            {
                return NotFound();
            }
            _context.Entry(post).Reference(b => b.Company).Load();
            var company = post.Company;
            if (company.OwnerId != user.Id)
            {
                return BadRequest("You can't delete post since you are not the owner of the page.");
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePost(string id, PostDTO dto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, post);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PostExists(id))
            {
                return NotFound();
            }

            return Ok();
        }


        private bool PostExists(string id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
