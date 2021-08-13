using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;

namespace diga.dal.DbServices.PlatformUserLanguageDbServices
{
    public class PlatformUserLanguageDbService : ManyToManyDbService<PlatformUserLanguage>, IPlatformUserLanguageDbService
    {
        private readonly DigaContext _db;

        public PlatformUserLanguageDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
