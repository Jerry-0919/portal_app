using diga.web.Models.PlatformCategories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCategoryServices
{
    public interface IPlatformCategoryService
    {
        Task<List<PlatformCategoryDto>> List();
        Task<List<PlatformCategoryDto>> ListPublished();
    }
}
