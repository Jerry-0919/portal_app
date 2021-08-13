using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractBidDbServices
{
    public class PlatformContractBidDbService : ManyToManyDbService<PlatformContractBid>, IPlatformContractBidDbService
    {
        private readonly DigaContext _db;

        public PlatformContractBidDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int userId, int contractId)
        {
            return await _db.PlatformContractBids.AnyAsync(b => b.ApplicationUserId == userId && b.PlatformContractId == contractId);
        }

        public async Task<PlatformContractBid> Get(int userId, int contractId)
        {
            return await _db.PlatformContractBids.FirstOrDefaultAsync(b => b.ApplicationUserId == userId && b.PlatformContractId == contractId);
        }

        public async Task<PlatformContractBid> GetAccepted(int contractId)
        {
            return await _db.PlatformContractBids.FirstOrDefaultAsync(b => b.PlatformContractId == contractId && b.Status == PlatformContractBidStatuses.Accepted);
        }

        public async Task<PlatformContractBid> GetFull(int userId, int contractId)
        {
            return await _db.PlatformContractBids.Include(b => b.PlatformContract)
                .Include(b => b.ApplicationUser).ThenInclude(u => u.Ratings)
                .FirstOrDefaultAsync(b => b.ApplicationUserId == userId && b.PlatformContractId == contractId);
        }

        public async Task<List<PlatformContractBid>> List(int contractId)
        {
            return await _db.PlatformContractBids.Include(b => b.ApplicationUser).ThenInclude(u => u.Ratings)
                .Where(b => b.PlatformContractId == contractId && b.Status != PlatformContractBidStatuses.Draft).ToListAsync();
        }
    }
}
