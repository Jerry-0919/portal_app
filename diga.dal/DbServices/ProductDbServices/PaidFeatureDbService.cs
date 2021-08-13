using diga.bl.Models.Platform.PaidFeatures;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.ProductDbServices
{
    public class PaidFeatureDbService : DefaultDbService<int, PaidFeature>, IPaidFeatureDbService
    {
        private readonly DigaContext _db;

        public PaidFeatureDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public Task<List<PaidFeature>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<PaidFeature>> GetPaginated(int skip, int take)
        {
            IQueryable<PaidFeature> query = _db.PaidFeatures.AsQueryable();

            return await query.Skip(skip).Take(take).ToListAsync();
        }
    }
}
