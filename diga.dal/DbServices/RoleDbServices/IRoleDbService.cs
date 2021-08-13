using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.RoleDbServices
{
    public interface IRoleDbService : IDefaultDbService<int, ApplicationRole>
    {
        public Task<ApplicationRole> Get(string name);
    }
}
