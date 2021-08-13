using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformAlbumCategoryDbServices
{
    public interface IPlatformAlbumCategoryDbService : IManyToManyDbService<PlatformAlbumCategory>
    {
        Task<List<PlatformAlbumCategory>> List(int id);
    }
}