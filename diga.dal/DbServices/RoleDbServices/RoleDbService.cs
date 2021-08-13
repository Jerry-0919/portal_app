using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.RoleDbServices
{
    public class RoleDbService : DefaultDbService<int, ApplicationRole>, IRoleDbService
    {
        private readonly DigaContext _db;

        public RoleDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationRole> Get(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
