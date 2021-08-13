using diga.web.Models.Pagination;
using diga.web.Models.PlatformPayServices;
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
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/user/bankAccounts")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformUserBankAccountController : BaseController
    {
        private readonly IMangoPayUserService _mangoPayUserService;

        public PlatformUserBankAccountController(IMangoPayUserService mangoPayUserService)
            : base(null, null)
        {
            _mangoPayUserService = mangoPayUserService;
        }

        [HttpGet("{page}/{perPage}")]
        public async Task<PaginatedList<BankAccountDto>> GetAccounts(int page, int perPage)
        {
            return await _mangoPayUserService.BankAccounts(UserId, page, perPage);
        }

        [HttpPost]
        public async Task<ResponseData> Post(BankAccountIbanPostDTO request)
        {
            return await _mangoPayUserService.RegisterBankAccount(UserId, request);
        }
    }
}
