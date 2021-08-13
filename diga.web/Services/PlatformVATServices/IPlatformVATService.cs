using diga.web.Models.PlatformVATs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformVATServices
{
    public interface IPlatformVATService
    {
        Task<List<PlatformVATDto>> List();
    }
}