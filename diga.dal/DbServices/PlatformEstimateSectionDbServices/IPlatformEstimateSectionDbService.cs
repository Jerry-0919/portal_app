using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimateSectionDbServices
{
    public interface IPlatformEstimateSectionDbService : IDefaultDbService<int, PlatformEstimateSection>
    {
        Task<List<PlatformEstimateSection>> List(int estimateId);
        Task<List<PlatformEstimateSection>> ListByIds(List<int> sectionIdsToRemove);
    }
}
