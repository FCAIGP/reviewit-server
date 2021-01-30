using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult AddReviewView(string id)
        {
            var model = new ReviewDto();
            model.CompanyId = id;
            return View(model);
        }
        public async Task<IActionResult> CreateReview(ReviewDto review)
        {
            if (ModelState.IsValid)
            {
                //TODO: figure out a way to get user ip
                //TODO: get user logged in info if opted in
                string ip = "40.200.170.60";
                Review newReview = Review.CreateFromDto(review, ip);

                CompanyPage mine = await _context.CompanyPage.FindAsync(review.CompanyId);
                await _context.Review.AddAsync(newReview);
                mine.Reviews.Add(newReview);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CompanyPages", new RouteValueDictionary { { "id", review.CompanyId } });
            }
            return View("AddReviewView");
        }
    }
}
