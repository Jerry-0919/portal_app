using diga.web.Models.PlatformCities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCityServices
{
    public interface IPlatformCityService
    {
        Task<List<PlatformCityDto>> List();
    }
}
