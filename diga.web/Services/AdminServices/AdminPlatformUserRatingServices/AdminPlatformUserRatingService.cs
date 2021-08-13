using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformUserRatingServices
{
    public class AdminPlatformUserRatingService : IAdminPlatformUserRatingService
    {
        private readonly IPlatformUserRatingDbService _platformUserRatingDbService;

        public AdminPlatformUserRatingService(IPlatformUserRatingDbService platformUserRatingDbService)
        {
            _platformUserRatingDbService = platformUserRatingDbService;
        }

        public async Task<PaginatedList<AdminPlatformUserRatingDto>> GetPaginatedList(int skip, int take, int userId)
        {
            return new PaginatedList<AdminPlatformUserRatingDto>
            {
                List = (await _platformUserRatingDbService.List(skip, take, userId))
                    .Select(r => new AdminPlatformUserRatingDto
                    {
                        UserId = r.ApplicationUserId,
                        UserName = r.ApplicationUser.Name,
                        DateTime = r.DateTime,
                        Description = r.Description,
                        Points = r.Points
                    }).ToList(),
                CountAll = await _platformUserRatingDbService.Count(userId)
            };
        }
    }
}
