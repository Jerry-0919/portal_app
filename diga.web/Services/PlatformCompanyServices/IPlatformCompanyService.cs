using diga.web.Models.PlatformCompanies;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCompanyServices
{
    public interface IPlatformCompanyService
    {
        Task<PlatformCompanyDto> Get(int userId);
        Task<ResponseData> Edit(int userId, PlatformCompanyEditDto request);
    }
}