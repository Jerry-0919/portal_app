using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformBalanceDbServices
{
    public interface IPlatformBalanceDbService : IDefaultDbService<int, PlatformBalance>
    {
        Task Withdrawal(int userId, double debited);
    }
}
