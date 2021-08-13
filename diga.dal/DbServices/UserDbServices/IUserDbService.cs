using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.UserDbServices
{
    public interface IUserDbService : IDefaultDbService<int, ApplicationUser>
    {
        Task<ApplicationUser> GetFull(string email);
        Task<ApplicationUser> GetFull(int id);
        Task<List<ApplicationUser>> List(int skip, int take);
        Task<bool> Any(string email);
        Task<ApplicationUser> GetProfile(int id);
        Task<ApplicationUser> Get(string email);
        Task<ApplicationUser> GetByPasswordCode(string code);
        Task<List<ApplicationUser>> ListByRole(string roleName);

        Task<ApplicationUser> GetWithVerifications(int id);
        Task<List<ApplicationUser>> ListWithVerifications(int skip, int take, bool verificationRequested);
        Task<int> Count(bool verificationRequested);
    }
}
