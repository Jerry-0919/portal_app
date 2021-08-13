using diga.web.Models.Pagination;
using diga.web.Models.PlatformPayServices;
using diga.web.Models.PlatformUserKYC;
using diga.web.Models.Status;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.MangoPayServices
{
    public interface IMangoPayUserService
    {
        Task<ResponseData> CreateWallet(int userId);
        Task<PaginatedList<CardDto>> Cards(int userId, int page, int perPage);
        Task<CardRegistrationDTO> RegisterCard(int userId);
        Task<ResponseData> UpdateCardRegistration(int userId, string registrationId, CardRegistrationEditDto request);        
        Task<PaginatedList<BankAccountDto>> BankAccounts(int userId, int page, int perPage);
        Task<ResponseData> RegisterBankAccount(int userId, BankAccountIbanPostDTO request);
        Task<ResponseData> CreateKYCCompany(int userId, PlatformUserKYCCompanyDto request);
        Task<ResponseData> CreateKYCIndividual(int userId, PlatformUserKYCIndividualDto request);
    }
}
