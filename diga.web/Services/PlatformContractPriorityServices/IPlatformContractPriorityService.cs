using diga.web.Models.PlatformContractPriorities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractPriorityServices
{
    public interface IPlatformContractPriorityService
    {
        Task<List<PlatformContractPriorityDto>> List();
    }
}
