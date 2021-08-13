using diga.bl.Models.Platform.PlatformPurchases;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPurchaseDbServises
{
    public class PlatformPurchaseDbService : DefaultDbService<int, PlatformPurchase>, IPlatformPurchaseDbService
    {
        private readonly DigaContext _db;

        public PlatformPurchaseDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformPurchase>> GetByUserId(int userId)
        {
           var purchases = await _db.PlatformPurchases.Where(x =>
                 x.UserId == userId)
                 .ToListAsync();
           return purchases;
        }

        public Task<List<PlatformPurchase>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlatformPurchase>> GetPaginated(int skip, int take, int userId)
        {
            IQueryable<PlatformPurchase> query = _db.PlatformPurchases.Where(x =>
                 x.UserId == userId).AsQueryable();

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<PlatformPurchase>> ListMangoPayOuts()
        {
            return await _db.PlatformPurchases.Where(p => p.Status == bl.Enums.PlatformPurchases.PaymentStatus.Created && p.PaymentProvider == bl.Enums.PlatformPurchases.PaymentProvider.MangoPayOut).ToListAsync();
        }

        public async Task<List<PlatformPurchase>> ListMangoPayTransfers()
        {
            return await _db.PlatformPurchases.Include(p => p.ParentPlatformPurchase).Where(p => p.Status == bl.Enums.PlatformPurchases.PaymentStatus.Created && p.PaymentProvider == bl.Enums.PlatformPurchases.PaymentProvider.MangoPayTransfer).ToListAsync();
        }
    }

}
