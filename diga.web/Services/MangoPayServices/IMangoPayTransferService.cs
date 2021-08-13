using MangoPay.SDK.Entities.GET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.MangoPayServices
{
    public interface IMangoPayTransferService
    {
        Task<TransferDTO> Make(int creditedUserId, int debitedUserId, long amount);
    }
}
