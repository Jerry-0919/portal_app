using diga.bl.Constants;
using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformCategoryDbServices
{
    public class PlatformCategoryDbService : DefaultDbService<int, PlatformCategory>, IPlatformCategoryDbService
    {
        private readonly DigaContext _db;

        public PlatformCategoryDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> AllExists(List<int> categoryIds)
        {
            return await _db.PlatformCategories.CountAsync(pc => categoryIds.Contains(pc.Id)) == categoryIds.Count;
        }

        public async Task<List<PlatformCategory>> List(int skip, int take)
        {
            return await _db.PlatformCategories.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<PlatformCategory>> ListParent(int skip, int take)
        {
            return await _db.PlatformCategories.Where(c => c.ParentCategoryId == null).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<PlatformCategory>> ListByParent(int parentCategoryId)
        {
            return await _db.PlatformCategories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
        }

        public async Task<List<PlatformCategory>> List(List<int> categories)
        {
            return await _db.PlatformCategories.Where(c => categories.Contains(c.Id)).ToListAsync();
        }

        public async Task<List<PlatformCategory>> ListPublished()
        {
            return await _db.PlatformContracts.Include(c => c.Categories)
                .Where(c => c.Status == PlatformContractStatuses.Contractor)
                .SelectMany(c => c.Categories.Select(ca => ca.PlatformCategory)).ToListAsync();
        }
    }
}
