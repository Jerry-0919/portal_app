using diga.bl.Models.Platform.PlatformPurchases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPurchaseServices
{
    public interface IPlatformPurchaseService
    {
        Task<List<PlatformPurchase>> GetAll();
        Task<List<PlatformPurchase>> GetAllForUser(int userId);
        Task<List<PlatformPurchase>> GetPaginatedList(int skip, int take, int userId);
    }
}
