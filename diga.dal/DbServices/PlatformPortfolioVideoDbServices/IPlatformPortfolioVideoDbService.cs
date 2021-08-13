using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPortfolioVideoDbServices
{
    public interface IPlatformPortfolioVideoDbService : IDefaultDbService<int, PlatformPortfolioVideo>
    {
        Task<List<PlatformPortfolioVideo>> List(int userId, int skip, int take);
        Task<List<PlatformPortfolioVideo>> List(int userId);
        Task<int> Count(int userId);
    }
}