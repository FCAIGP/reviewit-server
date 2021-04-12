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
    public class ClaimRequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClaimRequestController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<IEnumerable<ClaimRequestView>>> GetClaimRequestList()
        {
            return await _context.ClaimRequests.Select(claimRequest => _mapper.Map<ClaimRequestView>(claimRequest)).ToListAsync();
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ClaimRequestView>> GetClaimRequest(string id)
        {
            var claimRequest = await _context.ClaimRequests.FindAsync(id);

            if (claimRequest == null)
            {
                return NotFound();
            }

            return _mapper.Map<ClaimRequestView>(claimRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CompanyView>> CreateClaimRequest(ClaimRequestDTO dto)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest("An authentication error has been encountered!");
            }
            var company = await _context.Companies.FindAsync(dto.CompanyID);
            company.PendingStatusChange = true;
            var claimRequest = _mapper.Map<ClaimRequest>(dto);
            claimRequest.Submitter = user;
            claimRequest.ClaimStatus = ClaimStatus.Pending;
            claimRequest.CompanyId = dto.CompanyID;

            company.ClaimRequestsHistory.Add(claimRequest);
            await _context.ClaimRequests.AddAsync(claimRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimRequest", new { id = claimRequest.ClaimRequestId }, _mapper.Map<ClaimRequestView>(claimRequest));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/accept")]
        public async Task<ActionResult> Accept(string id)
        {
            return await ChangeStatus(id, ClaimStatus.Approved);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/reject")]
        public async Task<ActionResult> Reject(string id)
        {
            return await ChangeStatus(id, ClaimStatus.Rejected);
        }

        [NonAction]
        public async Task<ActionResult> ChangeStatus(string id, ClaimStatus newStatus)
        {
            var claimRequest = await _context.ClaimRequests.FindAsync(id);
            if(claimRequest == null)
            {
                return BadRequest("Couldn't find a request with given id");
            }

            if(claimRequest.ClaimStatus == ClaimStatus.Pending)
            {
                claimRequest.ClaimStatus = newStatus;
                
                if(newStatus == ClaimStatus.Approved)
                {
                    _context.Entry(claimRequest).Reference(b => b.Company).Load();
                    var company = claimRequest.Company;
                    company.AcceptedClaimRequest = claimRequest;
                    company.ClaimedDate = DateTime.Now;
                    company.OwnerId = claimRequest.SubmitterId;
                    company.PendingStatusChange = false;
                    await _context.SaveChangesAsync();

                    return Ok(new { action = "changed", message = $"Approved Claim Request." });
                }
                else if(newStatus == ClaimStatus.Rejected)
                {
                    await _context.SaveChangesAsync();
                    return Ok(new { action = "changed", message = $"Rejected Claim Request." });
                }

            }
            return Ok(new { action = "none", message = $"The Claim Request has already been {(claimRequest.ClaimStatus == ClaimStatus.Approved ? "Approved" : "Rejected")}" });
        }
    }
}
