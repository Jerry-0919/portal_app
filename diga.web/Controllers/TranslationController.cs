using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.web.Helpers;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using diga.dal.Services;

namespace diga.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private ICultureService CultureHelper { get; set; }
        private ILanguageStorage LanguageStorage { get; set; }
        private readonly DigaContext _context;
        public TranslationController(ICultureService ic, ILanguageStorage il, DigaContext context)
        {
            this.CultureHelper = ic;
            this.LanguageStorage = il;
            _context = context;
        }
        [HttpGet("get")]
        public IActionResult Get(string textId)
        {
            return Ok(CultureHelper.GetTranslation(textId, HttpContext.Session.GetString("SiteLanguage")));
        }

        [HttpGet("lang/{language}")]
        public IActionResult Language(string language)
        {
            var result = "{";

            foreach (var text in _context.Texts.Where(t => t.IsHtml != true).ToList())
            {
                switch (language)
                {
                    case "es":
                        result += $"\"{text.TextId}\": \"{text.Es.Replace("\"", "\\\"").Replace(System.Environment.NewLine, "").Replace("\n", "")}\",";
                        break;
                    case "en":
                        result += $"\"{text.TextId}\": \"{text.En.Replace("\"", "\\\"").Replace(System.Environment.NewLine, "").Replace("\n", "")}\",";
                        break;
                    case "pt":
                        result += $"\"{text.TextId}\": \"{text.Pt.Replace("\"", "\\\"").Replace(System.Environment.NewLine, "").Replace("\n", "")}\",";
                        break;
                    case "ru":
                        result += $"\"{text.TextId}\": \"{text.Ru.Replace("\"", "\\\"").Replace(System.Environment.NewLine, "").Replace("\n", "")}\",";
                        break;
                    default:
                        result += $"\"{text.TextId}\": \"{text.En.Replace("\"", "\\\"").Replace(System.Environment.NewLine, "").Replace("\n", "")}\",";
                        break;
                }                
            }

            result = result.Remove(result.Length - 1);
            result += "}";

            return Content(result, "application/json");
        }
    }
}