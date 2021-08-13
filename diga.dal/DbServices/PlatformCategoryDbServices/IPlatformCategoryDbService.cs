using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformCategoryDbServices
{
    public interface IPlatformCategoryDbService : IDefaultDbService<int, PlatformCategory>
    {
        Task<List<PlatformCategory>> List(int skip, int take);
        Task<List<PlatformCategory>> ListByParent(int parentCategoryId);
        Task<List<PlatformCategory>> ListParent(int skip, int take);

        Task<bool> AllExists(List<int> categoryIds);
        Task<List<PlatformCategory>> List(List<int> categories);
        Task<List<PlatformCategory>> ListPublished();
    }
}
