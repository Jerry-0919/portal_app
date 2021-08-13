using diga.dal;
using diga.web.Models.PaidFeatures;
using diga.web.Services.ProductServices;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/product")]
    [ApiController]
    public class PaidFeatureController : BaseController
    {
        private readonly IPaidFeatureService _productService;

        public PaidFeatureController(IPaidFeatureService productService)
            : base(null, null)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAll());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("{skip}/{take}")]
        public async Task<IActionResult> GetPaginatedList(int skip, int take)
        {
            return Ok(await _productService.GetPaginatedList(skip, take));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _productService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(PaidFeatureDto productModel) 
        {
            await _productService.Add(new PaidFeatureDto { 
                Currency = productModel.Currency,
                Description = productModel.Description,
                NameId = productModel.NameId,
                Price = productModel.Price
            });
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit(int id,PaidFeatureDto productModel)
        {
            await _productService.Edit(id,productModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            await _productService.Delete(id);

            return Ok();
        }


    }
}
