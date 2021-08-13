using diga.dal.DbServices.PlatformTagDbServices;
using diga.web.Models.PlatformTags;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformTagServices
{
    public class PlatformTagService : IPlatformTagService
    {
        private readonly IPlatformTagDbService _platformTagDbService;

        public PlatformTagService(IPlatformTagDbService platformTagDbService)
        {
            _platformTagDbService = platformTagDbService;
        }

        public async Task<List<PlatformTagDto>> List()
        {
            return (await _platformTagDbService.List())
                .Select(p => new PlatformTagDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
        }
    }
}
