using diga.bl.Models;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.UserRoleServices
{
    public interface IUserRoleDbService : IManyToManyDbService<ApplicationUserRole>
    {
        Task<List<ApplicationUserRole>> List(int userId);
        Task<ApplicationUserRole> Get(int userId, string role);
    }
}
