using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformCountryDbServices
{
    public class PlatformCountryDbService : DefaultDbService<int, PlatformCountry>, IPlatformCountryDbService
    {
        private readonly DigaContext _db;

        public PlatformCountryDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
