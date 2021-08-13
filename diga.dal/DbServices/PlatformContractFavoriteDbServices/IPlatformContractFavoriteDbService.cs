using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractFavoriteDbServices
{
    public interface IPlatformContractFavoriteDbService : IManyToManyDbService<PlatformContractFavorite>
    {
        Task<List<PlatformContractFavorite>> List(int userId, int skip, int take);
        Task<int> Count(int userId);
        Task<bool> Any(int userId, int platformContactId);
        Task<PlatformContractFavorite> Get(int userId, int platformContactId);
    }
}