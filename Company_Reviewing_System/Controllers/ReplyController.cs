using Company_Reviewing_System.Data;
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
    public class ReplyController : Controller
    {
        private readonly AppDbContext _context;
        public ReplyController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateReply(ReplyDto replyDto)
        {
            if (ModelState.IsValid)
            {
                User author = await _context.Users.FindAsync(User.Identity.GetId());
                Review parent = await _context.Review.FindAsync(replyDto.ReviewId);
                
                Reply reply = Reply.CreateFromDto(replyDto, parent, author);
                await _context.Reply.AddAsync(reply);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CompanyPages", new RouteValueDictionary { { "id", parent.Company.CompanyId } });
            }
            //This shouldn't happen
            return RedirectToAction("Index", "Home");
        }
    }
}
