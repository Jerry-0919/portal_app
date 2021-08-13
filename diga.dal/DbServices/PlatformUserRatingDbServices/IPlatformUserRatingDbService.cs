using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserRatingDbServices
{
    public interface IPlatformUserRatingDbService : IDefaultDbService<int, PlatformUserRating>
    {
        Task<int> CalculateRating(int userId);
        Task<bool> Any(int userId, string description);
        Task<List<PlatformUserRating>> List(int skip, int take, int userId);
        Task<int> Count(int userId);
    }
}