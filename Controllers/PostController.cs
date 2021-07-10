using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Returns a Post by specific id
        /// </summary>
        /// <param name="id">The id of the post to be fetched</param>
        /// <returns></returns>
        /// <response code="404">Returned if Post was not found</response>
        /// <response code="200">Returned if Post was fetched successfully</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PostView>> GetPost(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if(post == null)
            {
                return NotFound();
            }
            return _mapper.Map<PostView>(post);
        }

        /// <summary>
        /// Creates and adds a new Post to a Company Page (requires that user is the Owner of the company)
        /// </summary>
        /// <param name="dto">The data of the new Post</param>
        /// <response code = "400">Returned if authentication error occurs OR user is not owner of the company page</response>
        /// <response code = "201">Returned if request is successful and a new Post is added to system</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Deletes a Post with specific id (requires that user is Owner of the company)
        /// </summary>
        /// <param name="id">The id of the post to be deleted</param>
        /// <response code = "400">Returned if authentication error occurs Or user is not owner of the company page</response>
        /// <response code = "404">Returned if Post was not found</response>
        /// <response code = "204">Returned if system fulfilled request successfully</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Updates Post with specific id (requires that user is owner of the Company Page)
        /// </summary>
        /// <param name="id">The id of the Post to be updated</param>
        /// <param name="dto">The new Post data</param>
        /// <response code = "400">Returned if authentication error occurs OR user is not owner of Company Page</response>
        /// <response code="404">Returned if Post was not found</response>
        /// <response code="200">Returned if Post was successfully updated in the system</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePost(string id, PostDTO dto)
        {
            string ID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(ID);
            if (user == null)
            {
                return BadRequest("Authentication token doesn't correspond to a valid user.");
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Entry(post).Reference(b => b.Company).Load();
            var company = post.Company;
            if (company.OwnerId != user.Id)
            {
                return BadRequest("You can't update post since you are not the owner of the page.");
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
