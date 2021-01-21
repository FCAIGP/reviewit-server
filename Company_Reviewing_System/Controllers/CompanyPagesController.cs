using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;

namespace Company_Reviewing_System.Controllers
{
    public class CompanyPagesController : Controller
    {
        private readonly AppDbContext _context;

        public CompanyPagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CompanyPages
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.CompanyPage.Cast<CompanyPageDto>().ToListAsync());
        }

        // GET: CompanyPages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyPage = await _context.CompanyPage
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (companyPage == null)
            {
                return NotFound();
            }

            return View(companyPage);
        }

        // GET: CompanyPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyPageDto companyPage)
        {
            ModelState.Remove("CompanyId");
            if (ModelState.IsValid)
            {
                companyPage.CreatedDate = DateTime.Now;
                _context.Add(CompanyPage.CreateFrom(companyPage));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View((CompanyPageDto)companyPage);
        }

        // GET: CompanyPages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyPage = await _context.CompanyPage.FindAsync(id);
            if (companyPage == null)
            {
                return NotFound();
            }
            return View(companyPage);
        }

        // POST: CompanyPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CompanyId,Name,Headquarters,Industry,Region,CreatedDate,ClaimedDate,SubscribersEmails,LogoURL,PendingStatusChange,IsScoreUpToDate,Score,CloseStatus")] CompanyPage companyPage)
        {
            if (id != companyPage.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyPageExists(companyPage.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companyPage);
        }

        // GET: CompanyPages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyPage = await _context.CompanyPage
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (companyPage == null)
            {
                return NotFound();
            }

            return View(companyPage);
        }

        // POST: CompanyPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var companyPage = await _context.CompanyPage.FindAsync(id);
            _context.CompanyPage.Remove(companyPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyPageExists(string id)
        {
            return _context.CompanyPage.Any(e => e.CompanyId == id);
        }
    }
}
