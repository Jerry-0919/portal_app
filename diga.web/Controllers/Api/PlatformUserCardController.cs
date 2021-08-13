using diga.web.Models.Pagination;
using diga.web.Models.PlatformPayServices;
using diga.web.Models.PlatformUserKYC;
using diga.web.Models.Status;
using diga.web.Services.MangoPayServices;
using MangoPay.SDK.Entities.GET;
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
    [Route("api/platform/user/cards")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformUserCardController : BaseController
    {
        private readonly IMangoPayUserService _mangoPayUserService;

        public PlatformUserCardController(IMangoPayUserService mangoPayUserService)
            : base(null, null)
        {
            _mangoPayUserService = mangoPayUserService;
        }

        [HttpGet("{page}/{perPage}")]
        public async Task<PaginatedList<CardDto>> GetCards(int page, int perPage)
        {
            return await _mangoPayUserService.Cards(UserId, page, perPage);
        }

        [HttpPost("register")]
        public async Task<CardRegistrationDTO> RegisterCard()
        {
            return await _mangoPayUserService.RegisterCard(UserId);
        }

        [HttpPut("registrations/update/{registrationId}")]
        public async Task<ResponseData> UpdateCardRegistration(string registrationId, CardRegistrationEditDto request)
        {
            return await _mangoPayUserService.UpdateCardRegistration(UserId, registrationId, request);
        }
    }
}
