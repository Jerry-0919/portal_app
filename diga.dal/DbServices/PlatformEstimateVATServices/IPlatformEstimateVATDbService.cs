using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.ManyToManyDbServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimateVATServices
{
    public interface IPlatformEstimateVATDbService : IManyToManyDbService<PlatformEstimateVAT>
    {
        Task<List<PlatformEstimateVAT>> List(int estimateId);
    }
}
