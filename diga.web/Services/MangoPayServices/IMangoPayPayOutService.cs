using MangoPay.SDK.Entities.GET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.MangoPayServices
{
    public interface IMangoPayPayOutService
    {
        /// <summary>
        /// Выплата пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<PayOutBankWireDTO> ToBankAccount(int userId, long amount, string bankRef);
    }
}
