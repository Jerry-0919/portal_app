using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using diga.web.Helpers;
using diga.bl.Models;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using diga.dal;
using diga.dal.Services;

namespace diga.web.Controllers
{
    public class AccountController : Controller
    {
        private DigaContext db;
        private readonly ICultureService ch;
        private readonly ILanguageStorage ls;
        public AccountController(DigaContext context, ICultureService ch1, ILanguageStorage ls1)
        {
            db = context;
            this.ch = ch1;
            this.ls = ls1;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await db.Users
                    .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                var passwordHelper = new PasswordHelper();

                if (user != null)
                {
                    if (passwordHelper.Compare(model.Password, user.PasswordHash) == false){
                        throw new UnauthorizedAccessException();
                    }                            

                    await Authenticate(user);

                    if (user.UserRoles.Any(ur => ur.RoleId == 1))
                    {
                        return RedirectToAction("Index", "Administration");
                    }
                    if (user.UserRoles.Any(ur => ur.RoleId == 3))
                    {
                        return RedirectToAction("Index", "Platform");
                    }
                    if (user.UserRoles.Any(ur => ur.RoleId == 2))
                    {
                        return RedirectToAction("ForPartners", "Partner");
                    }

                    return RedirectToAction("Index", "Home");

                }
                ViewBag.ErrorMessage= ch.GetTranslation("user_not_exist", ls.GetLanguage());
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                var passwordHelper = new PasswordHelper();
                if (user == null)
                {
                    user = new ApplicationUser { Email = model.Email };

                    user.PasswordHash = passwordHelper.Generate(model.Password);

                    // Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Partner");
                    // if (userRole != null)
                    // {
                    //     user.RoleId = userRole.Id;
                    // }

                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("ForPartners", "Partner");
                }
                else
                    ViewBag.ErrorMessage = ch.GetTranslation("user_exist", ls.GetLanguage());
            }
            return View(model);
        }

        private async Task Authenticate(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim("Id", user.Id.ToString())
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Role.Name));
            }

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}