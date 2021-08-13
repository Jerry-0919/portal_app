using diga.dal.DbServices.PlatformCityDbServices;
using diga.web.Models.PlatformCities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCityServices
{
    public class PlatformCityService : IPlatformCityService
    {
        private readonly IPlatformCityDbService _platformCityDbService;

        public PlatformCityService(IPlatformCityDbService platformCityDbService)
        {
            _platformCityDbService = platformCityDbService;
        }

        public async Task<List<PlatformCityDto>> List()
        {
            return (await _platformCityDbService.List())
                .Select(c => new PlatformCityDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
        }
    }
}
