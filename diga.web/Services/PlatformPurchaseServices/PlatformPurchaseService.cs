using diga.bl.Models.Platform.PlatformPurchases;
using diga.dal.DbServices.PlatformPurchaseDbServises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPurchaseServices
{
    public class PlatformPurchaseService : IPlatformPurchaseService
    {

        private readonly IPlatformPurchaseDbService _purchaseDbService;

        public PlatformPurchaseService(IPlatformPurchaseDbService purchaseDbService)
        {
            _purchaseDbService = purchaseDbService;
        }

        public async Task<List<PlatformPurchase>> GetAll()
        {
            return await _purchaseDbService.List();
        }

        public async Task<List<PlatformPurchase>> GetAllForUser(int userId)
        {
            return await _purchaseDbService.GetByUserId(userId);
        }

        public async Task<List<PlatformPurchase>> GetPaginatedList(int skip, int take, int userId)
        {
            return await _purchaseDbService.GetPaginated(skip, take, userId);
        }
    }
}
