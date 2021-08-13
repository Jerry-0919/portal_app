using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserCategoryDbServices
{
    public interface IPlatformUserCategoryDbService : IManyToManyDbService<PlatformUserCategory>
    {
        Task<bool> Any(int userId, int categoryId);
        Task<PlatformUserCategory> Get(int userId, int categoryId);
        Task<List<PlatformUserCategory>> List(int userId);
    }
}