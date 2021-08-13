using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.DocumentServices.EstimateService
{
    public interface IDocumentEstimateService
    {
        public Task<byte[]> Pdf(int contractId, int userId, string language);
    }
}
