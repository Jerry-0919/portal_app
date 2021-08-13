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
    public class AdminQuestionsController : Controller
    {
        private readonly DigaContext _context;

        public AdminQuestionsController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminQuestions
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Questions.Include(q => q.AnswerText).Include(q => q.QuestionText).Include(q => q.FullAnswerText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .Include(q => q.AnswerText)
                .Include(q => q.QuestionText)
                .Include(q => q.FullAnswerText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        // GET: AdminQuestions/Create
        public IActionResult Create()
        {
            ViewData["AnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["QuestionTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["FullAnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId");

            return View();
        }

        // POST: AdminQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionTextId,AnswerTextId,FullAnswerTextId,Type")] Questions questions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.AnswerTextId);
            ViewData["QuestionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.QuestionTextId);
            ViewData["FullAnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.FullAnswerTextId);
            return View(questions);
        }

        // GET: AdminQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            ViewData["AnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.AnswerTextId);
            ViewData["QuestionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.QuestionTextId);
            ViewData["FullAnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.FullAnswerTextId);
            return View(questions);
        }

        // POST: AdminQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionTextId,AnswerTextId,FullAnswerTextId,Type")] Questions questions)
        {
            if (id != questions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(questions.Id))
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
            ViewData["AnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.AnswerTextId);
            ViewData["QuestionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.QuestionTextId);
            ViewData["FullAnswerTextId"] = new SelectList(_context.Texts, "TextId", "TextId", questions.FullAnswerTextId);
            return View(questions);
        }

        // GET: AdminQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .Include(q => q.AnswerText)
                .Include(q => q.QuestionText)
                .Include(q => q.FullAnswerText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        // POST: AdminQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questions = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(questions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionsExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
