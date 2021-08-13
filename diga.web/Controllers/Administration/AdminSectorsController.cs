using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using diga.bl.Models; 
using diga.dal;
using Microsoft.AspNetCore.Authorization;

namespace diga.web.Controllers.Administration
{
    [Authorize(Roles = "Admin")]
    public class AdminSectorsController : Controller
    {
        private readonly DigaContext _context;

        public AdminSectorsController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminSectors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sectors.ToListAsync());
        }

        // GET: AdminSectors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectors = await _context.Sectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectors == null)
            {
                return NotFound();
            }

            return View(sectors);
        }

        // GET: AdminSectors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminSectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BigTitleTextId,SmallTitleTextId,PictureUrl,LongTextId,Enabled")] Sectors sectors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sectors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sectors);
        }

        // GET: AdminSectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectors = await _context.Sectors.FindAsync(id);
            if (sectors == null)
            {
                return NotFound();
            }
            return View(sectors);
        }

        // POST: AdminSectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BigTitleTextId,SmallTitleTextId,PictureUrl,LongTextId,Enabled")] Sectors sectors)
        {
            if (id != sectors.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sectors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectorsExists(sectors.Id))
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
            return View(sectors);
        }

        // GET: AdminSectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectors = await _context.Sectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectors == null)
            {
                return NotFound();
            }

            return View(sectors);
        }

        // POST: AdminSectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sectors = await _context.Sectors.FindAsync(id);
            _context.Sectors.Remove(sectors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectorsExists(int id)
        {
            return _context.Sectors.Any(e => e.Id == id);
        }
    }
}
