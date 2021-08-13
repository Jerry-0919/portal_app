using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformUserPhoneNumberDbServices
{
    public class PlatformUserPhoneNumberDbService : DefaultDbService<int, PlatformUserPhoneNumber>, IPlatformUserPhoneNumberDbService
    {
        private readonly DigaContext _db;

        public PlatformUserPhoneNumberDbService(DigaContext db)
            : base(db)
        {

        }
    }
}
