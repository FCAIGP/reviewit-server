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

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyView>>> GetCompanyList()
        {
            return await _context.Companies.Select(company => _mapper.Map<CompanyView>(company)).ToListAsync();
        }

        // GET: api/Company/id
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyView>> GetCompany(string id)
        {
            var companyPage = await _context.Companies.FindAsync(id);

            if (companyPage == null)
            {
                return NotFound();
            }

            return _mapper.Map<CompanyView>(companyPage);
        }

        // PUT: api/Company/id
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
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

        // POST: api/Company
        [Authorize]
        [HttpPost]
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

        // DELETE: api/Company/id
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
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

        private bool CompanyExists(string id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }

        [HttpGet("{id}/reviews")]
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

        [HttpGet("{id}/posts")]
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
    }
}
