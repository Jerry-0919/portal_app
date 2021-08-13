using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminUserServices
{
    public interface IAdminUserService
    {
        Task<PaginatedList<AdminUserDto>> GetPaginatedList(int skip, int take, AdminPlatformUserFilterDto filter);
        Task<ResponseData> Update(int id, AdminUserEditDto request);
        Task<AdminUserFullDto> Get(int id);
    }
}
