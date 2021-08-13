using diga.bl.Models;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformCompanyDbServices
{
    public class PlatformCompanyDbService : DefaultDbService<int, PlatformCompany>, IPlatformCompanyDbService
    {
        private readonly DigaContext _db;

        public PlatformCompanyDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
