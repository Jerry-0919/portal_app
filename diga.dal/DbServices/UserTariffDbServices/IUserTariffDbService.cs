using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;
using diga.dal.DbServices.ManyToManyDbServices;

namespace diga.dal.DbServices.UserTariffDbServices
{
    public interface IUserTariffDbService : IManyToManyDbService<UserTariffs>
    {
    }
}
