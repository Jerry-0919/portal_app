using diga.bl.Models.Platform.MangoPay;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformMangoPayUserBankAccountDbServices
{
    public interface IPlatformMangoPayUserBankAccountDbService : IDefaultDbService<int, PlatformMangoPayUserBankAccount>
    {
    }
}
