using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;

namespace diga.dal.DbServices.PlatformUserFilterCategoryDbServices
{
    public class PlatformUserFilterCategoryDbService : ManyToManyDbService<PlatformUserFilterCategory>, IPlatformUserFilterCategoryDbService
    {
        public PlatformUserFilterCategoryDbService(DigaContext db)
            : base(db) { }
    }
}
