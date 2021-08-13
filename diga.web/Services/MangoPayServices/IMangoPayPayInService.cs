using diga.web.Models.Pagination;
using diga.web.Models.PlatformPayServices;
using diga.web.Models.Status;
using MangoPay.SDK.Entities.GET;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.MangoPayServices
{
    public interface IMangoPayPayInService
    {
        /// <summary>
        /// With card for simple users
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardId"></param>
        /// <param name="amount"></param>
        /// <param name="fee"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        Task<PayInCardDirectDTO> PayWithCard(int userId, string cardId, long amount, long fee, string returnUrl);
        /// <summary>
        /// For legal entitites
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<PayInBankWireDirectDTO> CreateBankWire(int userId, long amount, long fee);
    }
}
