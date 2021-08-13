using diga.bl.Models.Platform.MangoPay;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformMangoPayUserBankAccountDbServices
{
    public class PlatformMangoPayUserBankAccountDbService : DefaultDbService<int, PlatformMangoPayUserBankAccount>, IPlatformMangoPayUserBankAccountDbService
    {
        private readonly DigaContext _db;

        public PlatformMangoPayUserBankAccountDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
