using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformTagDbServices
{
    public interface IPlatformTagDbService : IDefaultDbService<int, PlatformTag>
    {
        Task<List<PlatformTag>> List(List<string> tags);
        Task<bool> Any(string tag);
        Task<PlatformTag> Get(string tag);
    }
}