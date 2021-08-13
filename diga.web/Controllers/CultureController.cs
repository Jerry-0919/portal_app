using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace diga.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        public ILanguageStorage LanguageStorage { get; set; }
        public CultureController(ILanguageStorage languageStorage)
        {
            this.LanguageStorage = languageStorage;
        }

        [HttpPost("set/{language}")]
        public ActionResult SetLanguage(string language)
        {
            LanguageStorage.SetLanguage(language);
            HttpContext.Session.SetString("SiteLanguage", language);
            return Ok();
        }
        
        [HttpGet("get")]
        public ActionResult GetLanguage()
        {            
            return Ok(HttpContext.Session.GetString("SiteLanguage"));
        }
    }
}