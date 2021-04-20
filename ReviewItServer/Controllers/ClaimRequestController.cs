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
    public class ClaimRequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClaimRequestController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of all Company Claim Requests (requires Admin priveleges)
        /// </summary>
        /// <response code="401">Returned if user is not Authorized to view Claim Requests</response>
        /// <response code="200">Returned when list of ClaimRequests is proberly fetched</response>
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClaimRequestView>>> GetClaimRequestList()
        {
            return await _context.ClaimRequests.Select(claimRequest => _mapper.Map<ClaimRequestView>(claimRequest)).ToListAsync();
        }
        
        /// <summary>
        /// Returns a Claim Request with specific id
        /// </summary>
        /// <param name="id">The id of the Claim Request</param>
        /// <response code="401">Returned if user is not Authorized to view Claim Request</response>
        /// <response code="404">Returned if no Claim Request was found</response>
        /// <response code="200">Returned when Claim Request is proberly fetched</response>
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ClaimRequestView>> GetClaimRequest(string id)
        {
            var claimRequest = await _context.ClaimRequests.FindAsync(id);

            if (claimRequest == null)
            {
                return NotFound();
            }

            return _mapper.Map<ClaimRequestView>(claimRequest);
        }

        /// <summary>
        /// Creates a new Claim Request and add it to the system (requires User priveleges)
        /// </summary>
        /// <param name="dto">The data of the Claim Request</param>
        /// <response code="401">Returned if user is not Authorized (not logged in)</response>
        /// <response code="400">Returned if Authentication error occurs</response>
        /// <response code="201">Returned when request is handled successfully and a new ClaimRequest is added to system</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CompanyView>> CreateClaimRequest(ClaimRequestDTO dto)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest("An authentication error has been encountered!");
            }
            var company = await _context.Companies.FindAsync(dto.CompanyID);
            
            var claimRequest = _mapper.Map<ClaimRequest>(dto);
            claimRequest.Submitter = user;
            claimRequest.ClaimStatus = ClaimStatus.Pending;
            claimRequest.CompanyId = dto.CompanyID;

            company.ClaimRequestsHistory.Add(claimRequest);
            await _context.ClaimRequests.AddAsync(claimRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimRequest", new { id = claimRequest.ClaimRequestId }, _mapper.Map<ClaimRequestView>(claimRequest));
        }

        /// <summary>
        /// Accepts a Claim Request with specific id (requires Admin priveleges)
        /// </summary>
        /// <param name="id">The id of the Claim Request to be Accepted</param>
        /// <response code="401">Returned if user is not Authorized</response>
        /// <response code="404">Returned if Claim Request is not found</response>
        /// <response code="200">Returned if Claim Request status is changed to Approved OR Request is already Approved</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/accept")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Accept(string id)
        {
            return await ChangeStatus(id, ClaimStatus.Approved);
        }

        /// <summary>
        /// Rejects a Claim Request with specific id (requires Admin priveleges)
        /// </summary>
        /// <param name="id">The id of the Claim Request to be Rejected</param>
        /// <response code="401">Returned if user is not Authorized</response>
        /// <response code="404">Returned if Claim Request is not found</response>
        /// <response code="200">Returned if Claim Request status is changed to Rejected OR Request is already Rejected</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/reject")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
