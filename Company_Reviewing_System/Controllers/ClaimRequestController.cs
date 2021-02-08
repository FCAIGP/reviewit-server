﻿using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Company_Reviewing_System.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Controllers
{
    public class ClaimRequestController : Controller
    {
        private readonly AppDbContext _context;
        public ClaimRequestController(AppDbContext context)
        {
            _context = context;
        }
        

        public async Task<IActionResult> CreateClaimRequest(ClaimRequestDto request, string id)
        {
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
    }
}
