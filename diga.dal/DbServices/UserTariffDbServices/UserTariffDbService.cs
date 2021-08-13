using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;
using diga.dal.DbServices.ManyToManyDbServices;

namespace diga.dal.DbServices.UserTariffDbServices
{
    public class UserTariffDbService : ManyToManyDbService<UserTariffs>,
        IUserTariffDbService
    {
        private readonly DigaContext _db;

        public UserTariffDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

    }
}
