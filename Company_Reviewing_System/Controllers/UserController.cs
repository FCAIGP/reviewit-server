using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reviewing_System.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Profile(string? id)
        {
            if (id == null)
                return NotFound();

            User user = await _context.Users.FirstOrDefaultAsync(m => m.UserName.ToLower() == id.ToLower());
            if (user == null)
                return NotFound();

            return View((UserDto)user);
        }
    }
}
