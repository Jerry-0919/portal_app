using diga.web.Models.Pagination;
using diga.web.Models.PlatformPortfolioAlbums;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPortfolioAlbumServices
{
    public interface IPlatformPortfolioAlbumService
    {
        Task<PaginatedList<PlatformPortfolioAlbumDto>> List(int userId, int skip, int take);
        Task<ResponseData> Add(int userId, PlatformPortfolioAlbumAddDto request);
        Task<ResponseData> Edit(int userId, int albumId, PlatformPortfolioAlbumEditDto request);
        Task<ResponseData> Remove(int userId, int albumId);
    }
}