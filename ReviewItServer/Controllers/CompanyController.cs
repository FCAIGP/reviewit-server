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
using ReviewItServer.Utility;
using ReviewItServer.ViewModels;

namespace ReviewItServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CompanyController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of all companies in the system. In the future, this will be limited to a certain maximum number of returned items.
        /// </summary>
        /// <response code="200">Returned when the company list is properly fetched.</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompanyView>>> GetCompanyList()
        {
            return await _context.Companies.Select(company => _mapper.Map<CompanyView>(company)).ToListAsync();
        }

        /// <summary>
        /// Returns a specific company by id.
        /// </summary>
        /// <param name="id">The id of the company to be fetched</param>
        /// <response code="200">Returned when the company is properly fetched.</response>  
        /// <response code="404">Returned when no company exists with the given id.</response>  
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompanyView>> GetCompany(string id)
        {
            var companyPage = await _context.Companies.FindAsync(id);

            if (companyPage == null)
            {
                return NotFound();
            }

            return _mapper.Map<CompanyView>(companyPage);
        }

        /// <summary>
        /// Updates a specific company by id. (requires admin privilege)
        /// </summary>
        /// <param name="id">The id of the company to be updated</param>
        /// <param name="dto">The new data of the company</param>
        /// <response code="200">Returned when the company is properly updated.</response>  
        /// <response code="404">Returned when no company exists with the given id.</response>  
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCompany(string id, CompanyDTO dto)
        {
            var companyPage = await _context.Companies.FindAsync(id);
            if (companyPage == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, companyPage);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CompanyExists(id))
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Creates and adds a new company to the system (requires user privilege).
        /// </summary>
        /// <param name="dto">The data of the new company</param>
        /// <response code="400">Returned when an authentication error occurs.</response>  
        /// <response code="201">Returned when the company is created and added to the system sucessfully.</response>  
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CompanyView>> CreateCompany(CompanyDTO dto)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest("Authentication token doesn't correspond to a valid user.");
            }
            var company = _mapper.Map<Company>(dto);
            company.Owner = user;
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, _mapper.Map<CompanyView>(company));
        }

        /// <summary>
        /// Deletes a specific company by id (requires admin privilege)
        /// </summary>
        /// <param name="id">The id of the company to be deleted</param>
        /// <response code="404">Returned when no company exists with the given id.</response>  
        /// <response code="204">Returned when the company is deleted successfully.</response>  
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns a list of all reviews on a specific company by id
        /// </summary>
        /// <param name="id">The id of the company</param>
        /// <response code="404">Returned when no company exists with the given id.</response>  
        /// <response code="200">Returned when the review list is properly fetched.</response>  
        [HttpGet("{id}/reviews")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewView>>> GetReviews(string id)
        {
            var companyPage = await _context.Companies.FindAsync(id);
            if (companyPage == null)
            {
                return NotFound();
            }
            await _context.Entry(companyPage).Collection(v => v.Reviews).LoadAsync();
            return companyPage.Reviews.Select(v=>_mapper.Map<ReviewView>(v)).ToList();
        }

        /// <summary>
        /// Returns a list of all posts on a specific company by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">Returned when no company exists with the given id.</response>  
        /// <response code="200">Returned when the post list is properly fetched.</response>  
        [HttpGet("{id}/posts")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PostView>>> GetPosts(string id)
        {
            var companyPage = await _context.Companies.FindAsync(id);
            if (companyPage == null)
            {
                return NotFound();
            }
            await _context.Entry(companyPage).Collection(v => v.Posts).LoadAsync();
            return companyPage.Posts.Select(v => _mapper.Map<PostView>(v)).ToList();
        }

        private bool CompanyExists(string id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
