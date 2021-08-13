using diga.web.Models.PlatformContractChanges;
using diga.web.Models.PlatformContractPaymentRequests;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractPaymentPartServices
{
    public interface IPlatformContractPaymentPartService
    {
        Task<List<PlatformContractPaymentPartDto>> List(int userId, int contractId);
        Task<ResponseData> EditInvoice(int userId, int contractId, int paymentPartId, PlatformContractPaymentRequestInvoiceEditDto request);
        Task<ResponseData> EditProof(int userId, int contractId, int paymentPartId, PlatformContractPaymentRequestProofEditDto request);
        Task<ResponseData> EditPaid(int userId, int contractId, int paymentPartId, PlatformContractPaymentRequestPaidEditDto request);
    }
}
