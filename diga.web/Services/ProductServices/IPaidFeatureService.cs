using diga.bl.Models.Platform.PaidFeatures;
using diga.web.Models.PaidFeatures;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.ProductServices
{
    public interface IPaidFeatureService
    {
        Task<PaidFeature> Get(int userId);
        Task<List<PaidFeature>> GetAll();
        Task<ResponseData> Add(PaidFeatureDto productModel);
        Task<ResponseData> Edit(int productId, PaidFeatureDto productModel);
        Task<ResponseData> Delete(int productId);
        Task<List<PaidFeature>> GetPaginatedList(int skip, int take);



    }
}
