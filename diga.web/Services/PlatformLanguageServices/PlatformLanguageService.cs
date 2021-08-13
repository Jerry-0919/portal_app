using diga.dal.DbServices.PlatformLanguageDbServices;
using diga.web.Models.PlatformLanguages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformLanguageServices
{
    public class PlatformLanguageService : IPlatformLanguageService
    {
        private readonly IPlatformLanguageDbService _platformLanguageDbService;

        public PlatformLanguageService(IPlatformLanguageDbService platformLanguageDbService)
        {
            _platformLanguageDbService = platformLanguageDbService;
        }

        public async Task<List<PlatformLanguageDto>> List()
        {
            return (await _platformLanguageDbService.List()).Select(l => new PlatformLanguageDto
            {
                Id = l.Id,
                Name = l.Name
            }).ToList();
        }
    }
}
