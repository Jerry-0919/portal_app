using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;

namespace diga.dal.DbServices.PlatformContractDiscussionDbServices
{
    public class PlatformContractDiscussionDbService : ManyToManyDbService<PlatformContractDiscussion>, IPlatformContractDiscussionDbService
    {
        public PlatformContractDiscussionDbService(DigaContext db)
            : base(db)
        {

        }
    }
}
