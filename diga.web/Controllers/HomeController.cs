using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using diga.web.Helpers;

namespace diga.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ILanguageStorage LanguageStorage { get; set; }

        public HomeController(ILogger<HomeController> logger, ILanguageStorage languageStorage)
        {
            _logger = logger;
            this.LanguageStorage = languageStorage;
        }
        
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SiteLanguage")))
            {
                LanguageStorage.SetLanguage("en");
                HttpContext.Session.SetString("SiteLanguage", "en");
            }else{
                LanguageStorage.SetLanguage(HttpContext.Session.GetString("SiteLanguage"));
            }
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Platform()
        {
            return View();
        }


    }
}
