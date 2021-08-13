using diga.web.Models.PlatformTags;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformTagServices
{
    public interface IPlatformTagService
    {
        Task<List<PlatformTagDto>> List();
    }
}