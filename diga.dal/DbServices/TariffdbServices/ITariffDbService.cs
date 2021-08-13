using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.TariffDbServices
{
    public interface ITariffDbService : IDefaultDbService<int, Tariffs>
    {
        public Task<Tariffs> Get(string name);
    }
}
