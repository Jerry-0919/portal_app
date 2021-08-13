using System;
using System.Linq;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace diga.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TextController:Controller
    {
        private DigaContext DigaContext { get; set; }
        public TextController(DigaContext context)
        {
            this.DigaContext = context;
        }
        [HttpGet]
        public IActionResult AllNonHtml()
        {
            var models = DigaContext.Texts.Where(t => t.IsHtml == false).ToList();
            ViewBag.models = models;
            return View();
        }

        [HttpGet]
        public IActionResult NonHtmlCreate()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HtmlCreate()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Post(Texts model)
        {
            DigaContext.Add(model);
            DigaContext.SaveChanges();

            if (model.IsHtml == true)
            {
                return RedirectToAction("AllHtml", "Text");
            }
            else
            {
                return RedirectToAction("AllNonHtml", "Text");
            }
        }

        [HttpGet]
        public IActionResult AllHtml()
        {
            var models = DigaContext.Texts.Where(t => t.IsHtml == true).ToList();
            ViewBag.models = models;
            return View();
        }
        [HttpPost]
        public IActionResult Delete(string textId, bool isHtml)
        {
            var text = DigaContext.Texts.Find(textId);

            if (text == null)
            {
                return NotFound();
            }

            DigaContext.Texts.Remove(text);
            DigaContext.SaveChanges();

            if (isHtml == true)
            {
                return RedirectToAction("AllHtml", "Text");
            }
            else
            {
                return RedirectToAction("AllNonHtml", "Text");
            }
        }
    }
}
