using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformCityDbServices
{
    public class PlatformCityDbService : DefaultDbService<int, PlatformCity>, IPlatformCityDbService
    {
        private readonly DigaContext _db;

        public PlatformCityDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
