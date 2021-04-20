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
    public class StatusChangeRequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public StatusChangeRequestController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns StatusChangeRequest with specific id (requires User priveleges)
        /// </summary>
        /// <param name="id">The id of the Request</param>
        /// <response code="401">Returned if user is not Authorized (not logged in)</response>
        /// <response code="404">Returned if request is not found</response>
        /// <response code="400">Returned if user is not Authorized (not Owner of the company page)</response>
        /// <response code="200">Returned when request is fetched successfully</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StatusChangeRequestView>> GetStatusChangeRequest(string id)
        {
            var request = await _context.StatusChangeRequests.FindAsync(id);
            if(request == null)
            {
                return NotFound();
            }
            _context.Entry(request).Reference(b => b.Company).Load();
            var company = request.Company;
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userID);
            if(company.OwnerId != user.Id)
            {
                return BadRequest("You're not Authorized to view the StatusChangeRequest");
            }
            return _mapper.Map<StatusChangeRequestView>(request);
        }

        /// <summary>
        /// Creates a new Status Change Request and add it to the system (requires User priveleges)
        /// </summary>
        /// <param name="dto">The data of the Request</param>
        /// <response code="401">User is not Authorized (not logged in)</response>
        /// <response code="400">Returned if Authentication error occured OR User is not Owner of Company Page</response>
        /// <response code="404">Returned if Company Page doesn't exist</response>
        /// <response code="201">Returned a new Status Change Request is created successfully and added to the system</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<StatusChangeRequestView>> CreateStatusChangeRequest(StatusChangeRequestDTO dto)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest("An authentication error has been encountered!");
            }
            var company = await _context.Companies.FindAsync(dto.CompanyId);
            if(company == null)
            {
                return NotFound();
            }
            if(company.OwnerId != user.Id)
            {
                return BadRequest("You're not Authorized to change the status of this company page");
            }

            var newRequest = _mapper.Map<StatusChangeRequest>(dto);
            newRequest.Date = DateTime.Now;
            company.StatusHistory.Add(newRequest);
            company.PendingStatusChange = true;
            // company.CloseStatus = newRequest.NewStatus;
            await _context.StatusChangeRequests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetStatusChangeRequest", new { id = newRequest.RequestId }, _mapper.Map<StatusChangeRequestView>(newRequest));
        }


        /// <summary>
        /// Accepts a Status Change Request with specific id (requires Admin priveleges)
        /// </summary>
        /// <param name="id">The id of Request</param>
        /// <response code="404">Returned if Request was not found</response>
        /// <response code="403">Returned if user is prohiibted from this action (Not Admin)</response>
        /// <response code="200">Returned Request is Accepted successfully OR Old status same as New status</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/accept")]
        public async Task<ActionResult> Accept(string id)
        {
            var statusChangeRequest = await _context.StatusChangeRequests.FindAsync(id);
            if(statusChangeRequest == null)
            {
                return NotFound();
            }
            _context.Entry(statusChangeRequest).Reference(b => b.Company).Load();
            var company = statusChangeRequest.Company;
            if(company.CloseStatus == statusChangeRequest.NewStatus)
            {
                return Ok(new { action = "none", message = $"No change in Company Page status, old status and new status are the same" });
            }
            CloseOptions oldStatus = company.CloseStatus;
            company.PendingStatusChange = false;
            company.CloseStatus = statusChangeRequest.NewStatus;
            await _context.SaveChangesAsync();
            return Ok(new { action = "changed", message = $"Changed Company Status from {getString(oldStatus)} to {statusChangeRequest.NewStatus}" });
        }


        /// <summary>
        /// Rejects a Status Change Request with specific id (requires Admin priveleges)
        /// </summary>
        /// <param name="id">The id of Request</param>
        /// <response code="404">Returned if Request was not found</response>
        /// <response code="403">Returned if user is prohiibted from this action (Not Admin)</response>
        /// <response code="200">Returned Request is rejected successfully</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}/reject")]
        public async Task<ActionResult> Reject(string id)
        {
            var statusChangeRequest = await _context.StatusChangeRequests.FindAsync(id);
            if (statusChangeRequest == null)
            {
                return NotFound();
            }
            _context.Entry(statusChangeRequest).Reference(b => b.Company).Load();
            var company = statusChangeRequest.Company;
            company.PendingStatusChange = false;
            await _context.SaveChangesAsync();
            // do nothing
            return Ok(new { action = "none", message = $"Rejected Status Change Request" });
        }

        [NonAction]
        public string getString(CloseOptions option)
        {
            string name = null;
            switch(option)
            {
                case CloseOptions.Open:
                    name = "Open";
                    break;
                case CloseOptions.ReOpened:
                    name = "ReOpened";
                    break;
                case CloseOptions.Erased:
                    name = "Erased";
                    break;
                case CloseOptions.ViewOnly:
                    name = "View Only";
                    break;
            }
            return name;
        }
    }
}
