using diga.bl.Models;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.UserRoleServices
{
    public class UserRoleDbService : ManyToManyDbService<ApplicationUserRole>, IUserRoleDbService
    {
        private readonly DigaContext _db;

        public UserRoleDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationUserRole> Get(int userId, string role)
        {
            return await _db.UserRoles.Include(ur => ur.Role).FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role.Name == role);
        }

        public async Task<List<ApplicationUserRole>> List(int userId)
        {
            return await _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == userId).ToListAsync();
        }
    }
}
