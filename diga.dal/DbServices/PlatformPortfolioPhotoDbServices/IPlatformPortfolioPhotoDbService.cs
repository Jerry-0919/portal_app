using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPortfolioPhotoDbServices
{
    public interface IPlatformPortfolioPhotoDbService : IDefaultDbService<int, PlatformPortfolioPhoto>
    {
        Task<List<PlatformPortfolioPhoto>> List(int albumId);
    }
}