using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformBalanceDbServices
{
    public class PlatformBalanceDbService : DefaultDbService<int, PlatformBalance>, IPlatformBalanceDbService
    {
        private readonly DigaContext _db;

        public PlatformBalanceDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task Withdrawal(int userId, double debited)
        {
            var contract = await _db.PlatformContracts.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var balance = await _db.PlatformBalances.Where(x => x.Id == contract.BalanceId).FirstOrDefaultAsync();
            balance.Value = balance.Value - debited;
            _db.PlatformBalances.Update(balance);
        }
    }
}
