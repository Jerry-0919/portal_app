using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using diga.web.Services.AdminServices.AdminPlatformContractServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/admin/platform/contracts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminPlatformContractController : ControllerBase
    {
        private readonly IAdminPlatformContractService _platformContractService;

        public AdminPlatformContractController(
            IAdminPlatformContractService platformContractService)
        {
            _platformContractService = platformContractService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<AdminPlatformContractDto>> ListPlatoformContracts(int skip, int take)
        {
            return await _platformContractService.GetPaginatedList(skip, take);
        }

        [HttpGet("{contractId}")]
        public async Task<AdminPlatformContractFullDto> GetPlatoformContract(int contractId)
        {
            return await _platformContractService.Get(contractId);
        }

        [HttpPost]
        public async Task<ResponseData> AddPlatformContract(AdminPlatformContractAddDto request)
        {
            return await _platformContractService.Add(request);
        }

        [HttpPut("{contractId}")]
        public async Task<ResponseData> UpdatePlatoformContract(int contractId, AdminPlatformContractEditDto request)
        {
            return await _platformContractService.Update(contractId, request);
        }

        [HttpDelete("{contractId}")]
        public async Task<ResponseData> RemovePlatoformContract(int contractId)
        {
            return await _platformContractService.Remove(contractId);
        }
    }
}
