using diga.dal.DbServices.PlatformVATDbServices;
using diga.web.Models.PlatformVATs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformVATServices
{
    public class PlatformVATService : IPlatformVATService
    {
        private readonly IPlatformVATDbService _platformVATDbService;

        public PlatformVATService(IPlatformVATDbService platformVATDbService)
        {
            _platformVATDbService = platformVATDbService;
        }

        public async Task<List<PlatformVATDto>> List()
        {
            return (await _platformVATDbService.List()).Select(v =>
                    new PlatformVATDto
                    {
                        Id = v.Id,
                        Code = v.Code,
                        Name = v.Name,
                        Percent = v.Percent,
                        RegionCode = v.RegionCode,
                        CountryId = v.CountryId
                    }).ToList();
        }
    }
}
