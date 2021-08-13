using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.TariffDbServices
{
    public class TariffDbService : DefaultDbService<int, Tariffs>, ITariffDbService
    {
        private readonly DigaContext _db;

        public TariffDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<Tariffs> Get(string name)
        {
            return await _db.Tariffs.Include(t => t.TariffModules).ThenInclude(tm => tm.Module).FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
