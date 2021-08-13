using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using diga.bl.Models; using diga.dal;
using diga.web.Auth;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using diga.web.Helpers;
using Microsoft.Extensions.Configuration;
using diga.web.Services.DocumentServices.EstimateService;

namespace diga.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IDocumentEstimateService _documentEstimateService;
        public PdfController(IDocumentEstimateService documentEstimateService)
        {
            _documentEstimateService = documentEstimateService;
        }

        [HttpGet("estimate/{contractId}/{userId}/{language}")]
        public async Task<IActionResult> Get(int contractId, int userId, string language)
        {
            var pdfFile = await _documentEstimateService.Pdf(contractId, userId, language);
            return File(pdfFile,
            "application/octet-stream", $"Orçamento {contractId}-{userId}.pdf");
        }

        //[MiddlewareFilter(typeof(JsReportPipeline))]
        //[HttpGet("estimate/{estimateId}/{language}")]
        //[AllowAnonymous]
        //public IActionResult Estimate(int estimateId, string language)
        //{
        //    var estimate = db.PlatformEstimates.Include(p => p.Sections).ThenInclude(s => s.Positions).FirstOrDefault(p => p.Id == estimateId);
        //    if (estimate == null)
        //    {
        //        return NotFound();
        //    }
        //    HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
        //    ViewBag.language = language;
        //    return View(estimate);
        //}

        //[MiddlewareFilter(typeof(JsReportPipeline))]
        //[HttpPost("invoice")]
        //public IActionResult Invoice([FromBody]InvoiceModel model)
        //{
        //    var cart = db.Carts.Find(model.CartId);
        //    if (cart==null || cart.UserId == null)
        //    {
        //        return BadRequest();
        //    }
        //    var user = db.Users.Find(cart.UserId.Value);
        //    ViewBag.user = user;
        //    ViewBag.cart = cart;
        //    ViewBag.data = model;
        //    ViewBag.priceForUsers = model.PriceForUsers;
        //    ViewBag.priceForModules = model.PriceForModules;
        //    HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
        //    return View();
        //}
    }
}
