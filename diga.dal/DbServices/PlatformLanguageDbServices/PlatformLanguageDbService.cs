using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformLanguageDbServices
{
    public class PlatformLanguageDbService : DefaultDbService<int, PlatformLanguage>, IPlatformLanguageDbService
    {
        private readonly DigaContext _db;

        public PlatformLanguageDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
