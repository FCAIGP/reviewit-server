using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetReviews(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var companyPage = await _context.CompanyPage
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if(companyPage == null)
            {
                return NotFound();
            }

            // return View(companyPage.Reviews);  *always null

            // testing
            ICollection<Review> coll = new Collection<Review>();
            coll.Add(new Review
            {
                ReviewId = "2",
                Body = "Really good company and good salary",
                Created = DateTime.Now,
                Contact = "ahmed@gmail.com",
                Salary = 2000,
                JobDescription = "Software Testing",
                
            });

            coll.Add(new Review
            {
                ReviewId = "3",
                Body = "Bad company and bad working enviroment",
                Created = DateTime.Now,
                Contact = "mohamed@hotmail.com",
                Salary = 1000,
                JobDescription = "Developer",

            });

            return View(coll);
        }
    }
}
