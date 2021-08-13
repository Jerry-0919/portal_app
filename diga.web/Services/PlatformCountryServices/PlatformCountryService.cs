using diga.dal.DbServices.PlatformCountryDbServices;
using diga.web.Models.PlatformCountries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCountryServices
{
    public class PlatformCountryService : IPlatformCountryService
    {
        private readonly IPlatformCountryDbService _platformCountryDbService;

        public PlatformCountryService(IPlatformCountryDbService platformCountryDbService)
        {
            _platformCountryDbService = platformCountryDbService;
        }

        public async Task<List<PlatformCountryDto>> List()
        {
            return (await _platformCountryDbService.List())
                .Select(c => new PlatformCountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code
                }).ToList();
        }
    }
}
