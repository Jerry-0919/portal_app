using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractDbServices
{
    public class PlatformContractCategoryDbService : ManyToManyDbService<PlatformContractCategory>, IPlatformContractCategoryDbService
    {
        private readonly DigaContext _db;

        public PlatformContractCategoryDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformContractCategory>> List(int contractId)
        {
            return await _db.PlatformContractCategories.Include(c => c.PlatformCategory)
                .Where(c => c.PlatformContractId == contractId).ToListAsync();
        }

        public async Task AddRange(int contractId, List<int> categoryIds)
        {
            await _db.PlatformContractCategories.AddRangeAsync(categoryIds.Select(c =>
                new PlatformContractCategory
                {
                    PlatformContractId = contractId,
                    PlatformCategoryId = c
                }));

            await _db.SaveChangesAsync();
        }
    }
}
