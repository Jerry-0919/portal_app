using diga.bl.Models.Platform.MangoPay;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace diga.dal.DbServices.PlatformMangoPayUserKYCDocumentDbService
{
    public interface IPlatformMangoPayUserKYCDocumentDbService : IDefaultDbService<int, PlatformMangoPayUserKYCDocument>
    {
    }
}
