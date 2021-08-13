using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformUserRatingServices
{
    public interface IAdminPlatformUserRatingService
    {
        Task<PaginatedList<AdminPlatformUserRatingDto>> GetPaginatedList(int skip, int take, int userId);
    }
}
