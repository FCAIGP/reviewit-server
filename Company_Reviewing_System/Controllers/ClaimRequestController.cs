using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Company_Reviewing_System.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Company_Reviewing_System.Controllers
{
    public class ClaimRequestController : Controller
    {
        private readonly AppDbContext _context;
        public ClaimRequestController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CreateClaimRequest(ClaimRequestDto request, string id)
        {
            ModelState.Remove("ClaimRequestId");
            if(ModelState.IsValid)
            {
                User submitter = await _context.Users.FindAsync(User.Identity.GetId());
                ClaimRequest newRequest = ClaimRequest.CreateFromDto(request, submitter);
                CompanyPage page = await _context.CompanyPage.FindAsync(id);
                await _context.ClaimRequest.AddAsync(newRequest);
                page.ClaimRequestsHistory.Add(newRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CompanyPages", new RouteValueDictionary { { "id", id } });
            }
            // shouldn't happen
            return View("CreateClaimRequest");
        }

        #region API Calls
        // GET: ViewPendingRequests
        [Authorize(Roles =Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> ViewPendingRequests()
        {
            return Json(new { data = await _context.ClaimRequest.Cast<ClaimRequestDto>().ToListAsync() });
            // return View(await _context.ClaimRequest.Where(x => x.ClaimStatus == ClaimStatus.Pending).Cast<ClaimRequestDto>().ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Accept(string id)
        {
            ClaimRequest request = await _context.ClaimRequest.FindAsync(id);
            request.ClaimStatus = ClaimStatus.Approved;
            await _context.SaveChangesAsync();
            return  Json(new { success = true, message = "Accepted the request" });
        }
        [HttpPost]
        public async Task<IActionResult> Reject(string id)
        {
            ClaimRequest request = await _context.ClaimRequest.FindAsync(id);
            request.ClaimStatus = ClaimStatus.Rejected;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Rejected the request" });
        }
        #endregion
    }
}
