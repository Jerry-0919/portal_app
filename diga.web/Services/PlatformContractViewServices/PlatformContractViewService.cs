using diga.bl.Models;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformContractViewDbServices;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractViewServices
{
    public class PlatformContractViewService : IPlatformContractViewService
    {
        private readonly IPlatformContractViewDbService _platformContractViewDbService;

        public PlatformContractViewService(IPlatformContractViewDbService platformContractViewDbService)
        {
            _platformContractViewDbService = platformContractViewDbService;
        }

        public async Task<ResponseData> Add(int userId, int contractId)
        {
            if (await _platformContractViewDbService.Any(userId, contractId))
                return new ResponseData();
            else
                await _platformContractViewDbService.Add(new PlatformContractView
                {
                    ApplicationUserId = userId,
                    PlatformContractId = contractId
                });

            return new ResponseData();
        }
    }
}
