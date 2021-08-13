using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPortfolioAlbumDbServices
{
    public interface IPlatformPortfolioAlbumDbService : IDefaultDbService<int, PlatformPortfolioAlbum>
    {
        Task<List<PlatformPortfolioAlbum>> List(int userId, int skip, int take);
        Task<int> Count(int userId);
    }
}