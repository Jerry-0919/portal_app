using diga.bl.Models.Platform.PaidFeatures;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.ProductDbServices
{
    public interface IPaidFeatureDbService : IDefaultDbService<int, PaidFeature>
    {
        //Task<Product> Get(int productId);
        Task<List<PaidFeature>> GetAll();
        Task<List<PaidFeature>> GetPaginated(int skip, int take);
    }
}
