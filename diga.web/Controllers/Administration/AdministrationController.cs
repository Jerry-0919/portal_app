using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace diga.web.Controllers.Administration
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}