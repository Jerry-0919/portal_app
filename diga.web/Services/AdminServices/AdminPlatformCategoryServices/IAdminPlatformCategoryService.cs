using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformCategoryServices
{
    public interface IAdminPlatformCategoryService
    {
        Task<PaginatedList<AdminPlatformCategoryDto>> GetPaginatedList(int skip, int take);
        Task<AdminPlatformCategoryDto> Get(int id);
        Task<ResponseData> Add(AdminPlatformCategoryAddDto request);
        Task<ResponseData> Update(int id, AdminPlatformCategoryEditDto request);
        Task<ResponseData> Remove(int id);
    }
}
