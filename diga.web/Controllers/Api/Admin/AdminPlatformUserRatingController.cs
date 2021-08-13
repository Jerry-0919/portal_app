using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Services.AdminServices.AdminPlatformUserRatingServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/admin/users/ratings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminPlatformUserRatingController : ControllerBase
    {
        private readonly IAdminPlatformUserRatingService _platformUserRatingService;

        public AdminPlatformUserRatingController(
            IAdminPlatformUserRatingService platformUserRatingService)
        {
            _platformUserRatingService = platformUserRatingService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<AdminPlatformUserRatingDto>> ListUserRatings(int skip, int take, int userId = 0)
        {
            return await _platformUserRatingService.GetPaginatedList(skip, take, userId);
        }
    }
}
