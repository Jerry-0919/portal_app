using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformVATDbServices
{
    public class PlatformVATDbService : DefaultDbService<int, PlatformVAT>, IPlatformVATDbService
    {
        public PlatformVATDbService(DigaContext db)
            : base(db) { }
    }
}
