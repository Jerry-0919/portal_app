using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformContractServices
{
    public interface IAdminPlatformContractService
    {
        Task<PaginatedList<AdminPlatformContractDto>> GetPaginatedList(int skip, int take);
        Task<AdminPlatformContractFullDto> Get(int id);
        Task<ResponseData> Add(AdminPlatformContractAddDto request);
        Task<ResponseData> Update(int id, AdminPlatformContractEditDto request);
        Task<ResponseData> Remove(int id);
    }
}
