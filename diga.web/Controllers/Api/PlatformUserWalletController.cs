using diga.web.Models.Status;
using diga.web.Services.MangoPayServices;
using MangoPay.SDK.Entities.POST;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/user/wallets")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformUserWalletController : BaseController
    {
        private readonly IMangoPayUserService _mangoPayUserService;

        public PlatformUserWalletController(IMangoPayUserService mangoPayUserService)
            : base(null, null)
        {
            _mangoPayUserService = mangoPayUserService;
        }

        [HttpPost]
        public async Task<ResponseData> Post()
        {
            return await _mangoPayUserService.CreateWallet(UserId);
        }
    }
}
