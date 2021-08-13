using diga.web.Models.PlatformBalances;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformBalanceServices
{
    public interface IPlatformBalanceService
    {
        Task<List<PlatformBalanceDto>> List();
        Task<ResponseData> Withdrawal(int userId, double debited);
    }
}
