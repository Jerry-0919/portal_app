using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Mvc;

namespace diga.web.Controllers
{
    public class HelpCenterController : Controller
    {
        private DigaContext DigaContext { get; set; }
        public HelpCenterController(DigaContext context)
        {
            this.DigaContext = context;
        }

        public IActionResult KnowledgeBase()
        {
            return View();
        }

        public IActionResult FAQ(int id)
        {
            var question = DigaContext.Questions.FirstOrDefault(q=>q.Id==id);
            return View(question);
        }
    }
}
