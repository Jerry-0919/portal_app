using diga.dal.DbServices.PlatformContractTypeDbServices;
using diga.web.Models.PlatformContractTypes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractTypeServices
{
    public class PlatformContractTypeService : IPlatformContractTypeService
    {
        private readonly IPlatformContractTypeDbService _platformContractTypeService;

        public PlatformContractTypeService(IPlatformContractTypeDbService platformContractTypeDbService)
        {
            _platformContractTypeService = platformContractTypeDbService;
        }

        public async Task<List<PlatformContractTypeDto>> List()
        {
            return (await _platformContractTypeService.List())
                .Select(t => new PlatformContractTypeDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();
        }
    }
}
