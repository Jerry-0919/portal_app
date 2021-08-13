using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace diga.web.Controllers
{
    public class PlatformController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PlatformController(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            ViewBag.isDebug = false;
            
// #if DEBUG
            if (_httpContextAccessor.HttpContext.Request.Host.Value.Contains("localhost")){
                ViewBag.isDebug = true;  
            }                
// #endif            
            return View();
        }
    }
}