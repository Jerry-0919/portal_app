using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using diga.bl.Models; using diga.dal;

namespace diga.web.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly DigaContext _context;

        public TeamMembersController(DigaContext context)
        {
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.TeamMembers.Include(t => t.CountryText).Include(t => t.DepartmentText).Include(t => t.NameText).Include(t => t.PositionText);
            return View(await digaContext.ToListAsync());
        }

        // GET: TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.CountryText)
                .Include(t => t.DepartmentText)
                .Include(t => t.NameText)
                .Include(t => t.PositionText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMembers == null)
            {
                return NotFound();
            }

            return View(teamMembers);
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["CountryTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["DepartmentTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameTextId,PhotoUrl,DepartmentTextId,PositionTextId,CountryTextId,ProfileUrl,ProfileIcon")] TeamMembers teamMembers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamMembers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.CountryTextId);
            ViewData["DepartmentTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.DepartmentTextId);
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.NameTextId);
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.PositionTextId);
            return View(teamMembers);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers.FindAsync(id);
            if (teamMembers == null)
            {
                return NotFound();
            }
            ViewData["CountryTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.CountryTextId);
            ViewData["DepartmentTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.DepartmentTextId);
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.NameTextId);
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.PositionTextId);
            return View(teamMembers);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameTextId,PhotoUrl,DepartmentTextId,PositionTextId,CountryTextId,ProfileUrl,ProfileIcon")] TeamMembers teamMembers)
        {
            if (id != teamMembers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamMembers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamMembersExists(teamMembers.Id))
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
            ViewData["CountryTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.CountryTextId);
            ViewData["DepartmentTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.DepartmentTextId);
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.NameTextId);
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", teamMembers.PositionTextId);
            return View(teamMembers);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.CountryText)
                .Include(t => t.DepartmentText)
                .Include(t => t.NameText)
                .Include(t => t.PositionText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMembers == null)
            {
                return NotFound();
            }

            return View(teamMembers);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMembers = await _context.TeamMembers.FindAsync(id);
            _context.TeamMembers.Remove(teamMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMembersExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
        }
    }
}
