using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimatePositionDbServices
{
    public interface IPlatformEstimatePositionDbService : IDefaultDbService<int, PlatformEstimatePosition>
    {
        Task<List<PlatformEstimatePosition>> List(int estimateId);
        Task<List<PlatformEstimatePosition>> List(List<int> sectionIds);
        Task<List<PlatformEstimatePosition>> ListByIds(List<int> positionIdsToRemove);
        Task<double> CalculatePrice(int estimateId);
    }
}
