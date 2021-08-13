using diga.web.Models.PlatformCompanies;
using diga.web.Models.Status;
using diga.web.Services.PlatformCompanyServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/companies")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformCompanyController : BaseController
    {
        private readonly IPlatformCompanyService _platformCompanyService;

        public PlatformCompanyController(IPlatformCompanyService platformCompanyService)
            : base(null, null)
        {
            _platformCompanyService = platformCompanyService;
        }

        [HttpGet("get")]
        public async Task<PlatformCompanyDto> Get()
        {
            return await _platformCompanyService.Get(UserId);
        }

        [HttpPut]
        public async Task<ResponseData> Edit(PlatformCompanyEditDto request)
        {
            return await _platformCompanyService.Edit(UserId, request);
        }
    }
}
