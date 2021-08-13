using diga.web.Models.PlatformLanguages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformLanguageServices
{
    public interface IPlatformLanguageService
    {
        Task<List<PlatformLanguageDto>> List();
    }
}