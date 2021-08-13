using diga.dal.DbServices.PlatformContractPriorityDbServices;
using diga.web.Models.PlatformContractPriorities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractPriorityServices
{
    public class PlatformContractPriorityService : IPlatformContractPriorityService
    {
        private readonly IPlatformContractPriorityDbService _platformContractPriorityDbService;

        public PlatformContractPriorityService(IPlatformContractPriorityDbService platformContractPriorityDbService)
        {
            _platformContractPriorityDbService = platformContractPriorityDbService;
        }

        public async Task<List<PlatformContractPriorityDto>> List()
        {
            return (await _platformContractPriorityDbService.List())
                .Select(c => new PlatformContractPriorityDto
                {
                    Id = c.Id,
                    Value = c.Value
                }).ToList();
        }
    }
}
