using diga.bl.Models;
using diga.dal.DbServices.RoleDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.dal.DbServices.UserRoleServices;
using diga.web.Models.Roles;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.RolesServices
{
    public class RoleService : IRoleService
    {
        private readonly IRoleDbService _roleService;
        private readonly IUserRoleDbService _userRoleDbService;
        private readonly IUserDbService _userDbService;

        public RoleService(IRoleDbService roleService, IUserRoleDbService userRoleDbService, IUserDbService userDbService)
        {
            _roleService = roleService;
            _userRoleDbService = userRoleDbService;
            _userDbService = userDbService;
        }

        public async Task<ResponseData> AddUserRole(int userId, string roleName)
        {
            List<string> customerRoles = new List<string> {
                "PlatformExecutorMaster",
                "PlatformExecutorTeam",
                "PlatformExecutorCompany"
            };

            List<string> allowedRoles = new List<string> {
                "PlatformCustomer"
            };

            allowedRoles.AddRange(customerRoles);

            if (!allowedRoles.Contains(roleName))
                return ResponseData.Fail("Role", "Role does not exists!");

            List<ApplicationUserRole> userRoles = await _userRoleDbService.List(userId);
            foreach (var userRole in userRoles)
            {
                if (customerRoles.Any(c => c == userRole.Role.Name))
                {
                    await _userRoleDbService.Remove(userRole);
                }
            }

            ApplicationRole role = await _roleService.Get(roleName);

            await _userRoleDbService.Add(new ApplicationUserRole
            {
                RoleId = role.Id,
                UserId = userId
            });

            if (roleName == "PlatformExecutorTeam" || roleName == "PlatformExecutorCompany")
            {
                var user = await _userDbService.Get(userId);
                user.IsCompany = true;
                await _userDbService.Update(user);
            }

            return new ResponseData();
        }

        public async Task<ResponseData> RemoveUserRole(int userId, string roleName)
        {
            ApplicationUserRole role = await _userRoleDbService.Get(userId, roleName);

            if (role != null)
                await _userRoleDbService.Remove(role);

            return new ResponseData();
        }

        public async Task<List<UserRoleDto>> List(int userId)
        {
            return (await _userRoleDbService.List(userId))
                .Select(ur => new UserRoleDto
                {
                    Role = ur.Role.Name
                }).ToList();
        }
    }
}
