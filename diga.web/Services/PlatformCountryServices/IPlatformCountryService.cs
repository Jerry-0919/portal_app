using diga.web.Models.PlatformCountries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCountryServices
{
    public interface IPlatformCountryService
    {
        Task<List<PlatformCountryDto>> List();
    }
}
