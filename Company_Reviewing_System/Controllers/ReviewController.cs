using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using Company_Reviewing_System.Utility;
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
                //TODO: Test RemoteIpAddress fetcher on an external sever instead of ISS
                string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                User? author = (User.Identity.IsAuthenticated && !review.IsAnonymous) ? await _context.Users.FindAsync(User.Identity.GetId()) : null;
                Review newReview = Review.CreateFromDto(review, ip, author);

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
