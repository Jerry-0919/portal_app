using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractFavoriteDbServices
{
    public class PlatformContractFavoriteDbService : ManyToManyDbService<PlatformContractFavorite>, IPlatformContractFavoriteDbService
    {
        private readonly DigaContext _db;

        public PlatformContractFavoriteDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformContractFavorite>> List(int userId, int skip, int take)
        {
            return await _db.PlatformContractFavorites
                .Include(f => f.PlatformContract).ThenInclude(c => c.Categories).ThenInclude(c => c.PlatformCategory)
                .Include(f => f.PlatformContract).ThenInclude(c => c.ContractType)
                .Where(f => f.ApplicationUserId == userId).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> Count(int userId)
        {
            return await _db.PlatformContractFavorites.CountAsync(f => f.ApplicationUserId == userId);
        }

        public async Task<bool> Any(int userId, int platformContactId)
        {
            return await _db.PlatformContractFavorites.AnyAsync(f => f.ApplicationUserId == userId && f.PlatformContractId == platformContactId);
        }

        public async Task<PlatformContractFavorite> Get(int userId, int platformContactId)
        {
            return await _db.PlatformContractFavorites.FirstOrDefaultAsync(f => f.ApplicationUserId == userId && f.PlatformContractId == platformContactId);
        }
    }
}
