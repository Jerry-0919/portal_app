using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformContractPriorityDbServices
{
    public class PlatformContractPriorityDbService : DefaultDbService<int, PlatformContractPriority>, IPlatformContractPriorityDbService
    {
        private readonly DigaContext _db;

        public PlatformContractPriorityDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
