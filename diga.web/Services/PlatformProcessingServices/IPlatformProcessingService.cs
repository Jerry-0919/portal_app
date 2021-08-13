using diga.web.Models.PlatformPayIns;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformProcessingServices
{
    public interface IPlatformProcessingService
    {
        Task<PlatformPayInDto> ProcessPayIn(int userId, PlatformPayInAddDto request);
        Task<ResponseData> ConfirmPayIn(int userId, PlatformPayInConfirmDto request);
    }
}
