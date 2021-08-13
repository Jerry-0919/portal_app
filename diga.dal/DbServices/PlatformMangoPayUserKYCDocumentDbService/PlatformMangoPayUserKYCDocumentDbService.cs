using diga.bl.Models.Platform.MangoPay;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace diga.dal.DbServices.PlatformMangoPayUserKYCDocumentDbService
{
    public class PlatformMangoPayUserKYCDocumentDbService : DefaultDbService<int, PlatformMangoPayUserKYCDocument>, IPlatformMangoPayUserKYCDocumentDbService
    {
        private readonly DigaContext _db;

        public PlatformMangoPayUserKYCDocumentDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
