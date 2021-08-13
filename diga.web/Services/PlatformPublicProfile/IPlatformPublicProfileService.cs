using diga.web.Models.PlatformPublicProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPublicProfile
{
    public interface IPlatformPublicProfileService
    {
        Task<PlatformPublicProfileDto> Get(int userId);
    }
}
