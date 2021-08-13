using diga.web.Models.PlatformContractTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractTypeServices
{
    public interface IPlatformContractTypeService
    {
        Task<List<PlatformContractTypeDto>> List();
    }
}
