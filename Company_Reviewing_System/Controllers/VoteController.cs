using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Company_Reviewing_System.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Controllers
{
    public class VoteController : Controller
    {
        private readonly AppDbContext _context;
        public VoteController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddVote(string reviewID, bool is_upvote)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FindAsync(User.Identity.GetId());
                Review review = await _context.Review.FindAsync(reviewID);
                // check like this for now
                bool alreadyVoted = false;
                foreach (var item in review.Votes)
                {
                    if (item.User == user)
                    {
                        alreadyVoted = true;
                        break;
                    }
                }
                if (alreadyVoted == false)
                {
                    Vote newVote = new Vote
                    {
                        User = user,
                        IsUpVote = is_upvote,
                        Review = review,
                    };
                    await _context.Vote.AddAsync(newVote);
                    review.Votes.Add(newVote);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, responseText = "Upvoted Successfuly" });

                }
                else
                {
                    return Json(new { success = true, responseText = "You have already Upvoted" });

                }
            }
            return Json(new { success = false, responseText = "Error happened while upvoting" });

        }
    }
}
