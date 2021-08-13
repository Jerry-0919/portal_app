using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractTypeDbServices
{
    public class PlatformContractTypeDbService : DefaultDbService<int, PlatformContractType>, IPlatformContractTypeDbService
    {
        private readonly DigaContext _db;

        public PlatformContractTypeDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(string name)
        {
            return await _db.PlatformContractTypes.AnyAsync(t => t.Name == name);
        }
    }
}
