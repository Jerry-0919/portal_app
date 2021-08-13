using diga.web.Models.PlatformUserKYC;
using diga.web.Models.Status;
using diga.web.Services.MangoPayServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/user/kyc")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformUserKYCController : BaseController
    {
        private readonly IMangoPayUserService _mangoPayUserService;

        public PlatformUserKYCController(IMangoPayUserService mangoPayUserService)
            : base(null, null)
        {
            _mangoPayUserService = mangoPayUserService;
        }

        [HttpPost("company")]
        public async Task<ResponseData> Post(PlatformUserKYCCompanyDto request)
        {
            return await _mangoPayUserService.CreateKYCCompany(UserId, request);
        }

        [HttpPost("individual")]
        public async Task<ResponseData> Post(PlatformUserKYCIndividualDto request)
        {
            return await _mangoPayUserService.CreateKYCIndividual(UserId, request);
        }
    }
}
