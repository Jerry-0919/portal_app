using diga.bl.Models.Platform.PlatformPurchases;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPurchaseDbServises
{
    public interface IPlatformPurchaseDbService : IDefaultDbService<int, PlatformPurchase>
    {

        Task<List<PlatformPurchase>> GetAll();
        Task<List<PlatformPurchase>> GetByUserId(int userId);
        Task<List<PlatformPurchase>> GetPaginated(int skip, int take, int userId);
        Task<List<PlatformPurchase>> ListMangoPayOuts();
        Task<List<PlatformPurchase>> ListMangoPayTransfers();
    }
}
