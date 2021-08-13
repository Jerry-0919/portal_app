using diga.dal.DbServices.PlatformContractDiscussionDbServices;

namespace diga.web.Services.PlatformContractDiscussionServices
{
    public class PlatformContractDiscussionService : IPlatformContractDiscussionService
    {
        private readonly IPlatformContractDiscussionDbService _platformContractDiscussionDbService;

        public PlatformContractDiscussionService(IPlatformContractDiscussionDbService platformContractDiscussionDbService)
        {
            _platformContractDiscussionDbService = platformContractDiscussionDbService;
        }


    }
}
