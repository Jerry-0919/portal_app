using diga.dal.DbServices.PlatformBalanceDbServices;
using diga.web.Models.PlatformBalances;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformBalanceServices
{
    public class PlatformBalanceService : IPlatformBalanceService
    {
        private readonly IPlatformBalanceDbService _platformBalanceDbService;

        public PlatformBalanceService(IPlatformBalanceDbService platformBalanceDbService)
        {
            _platformBalanceDbService = platformBalanceDbService;
        }

        public async Task<List<PlatformBalanceDto>> List()
        {
            return (await _platformBalanceDbService.List()).Select(b => new PlatformBalanceDto
            {
                Id = b.Id,
                Value = b.Value
            }).ToList();
        }

        public async Task<ResponseData> Withdrawal(int userId, double debited)
        {
            await _platformBalanceDbService.Withdrawal(userId, debited);
            return new ResponseData();
        }
    }
}
