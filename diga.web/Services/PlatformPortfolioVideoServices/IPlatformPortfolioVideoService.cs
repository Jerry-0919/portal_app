using diga.web.Models.Pagination;
using diga.web.Models.PlatformPortfolioVideos;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPortfolioVideoServices
{
    public interface IPlatformPortfolioVideoService
    {
        Task<PaginatedList<PlatformPortfolioVideoDto>> List(int userId, int skip, int take);
        Task<ResponseData> Add(int userId, PlatformPortfolioVideoAddDto video);
        Task<ResponseData> Edit(int userId, int albumId, PlatformPortfolioVideoEditDto videoId);
        Task<ResponseData> Remove(int userId, int albumId);
        Task<List<PlatformPortfolioVideoDto>> List(int userId);
    }
}