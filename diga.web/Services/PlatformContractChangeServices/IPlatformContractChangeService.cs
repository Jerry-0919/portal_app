using diga.web.Models.PlatformContractChanges;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractChangeServices
{
    public interface IPlatformContractChangeService
    {
        Task<Dictionary<DateTime, List<PlatformContractChangeDto>>> List(int userId, int contractId);
        Task<ResponseData<PlatformContractApprovalDto>> GetApproval(int userId, int contractId);
        Task<ResponseData> Approve(int userId, int contractId);
        Task<ResponseData> Edit(int userId, int contractId, PlatformContractChangeInfoDto request);

        Task<ResponseData> EditPaymentParts(int userId, int contractId, PlatformContractChangePaymentPartsDto request);
        Task<ResponseData> EditPaymentPartPercents(int userId, int contractId, PlatformContractChangePaymentPartPercentsDto request);
    }
}
