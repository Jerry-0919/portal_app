using diga.web.Models.Roles;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.RolesServices
{
    public interface IRoleService
    {
        Task<ResponseData> AddUserRole(int userId, string role);
        Task<ResponseData> RemoveUserRole(int userId, string role);
        Task<List<UserRoleDto>> List(int userId);
    }
}
