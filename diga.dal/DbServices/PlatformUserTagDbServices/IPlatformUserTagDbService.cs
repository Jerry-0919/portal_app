using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserTagDbServices
{
    public interface IPlatformUserTagDbService : IManyToManyDbService<PlatformUserTag>
    {
        Task<bool> Any(int userId, int tagId);
        Task<PlatformUserTag> Get(int userId, int tagId);
        Task<List<PlatformUserTag>> List(int userId);
        Task<int> Count(int userId);
        Task<bool> Any(int userId, string tag);
    }
}