using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformCompanyDbServices;
using diga.web.Helpers;
using diga.web.Models.PlatformCompanies;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCompanyServices
{
    public class PlatformCompanyService : IPlatformCompanyService
    {
        private readonly IPlatformCompanyDbService _platformCompanyDbService;

        public PlatformCompanyService(IPlatformCompanyDbService platformCompanyDbService)
        {
            _platformCompanyDbService = platformCompanyDbService;
        }

        public async Task<PlatformCompanyDto> Get(int userId)
        {
            PlatformCompany company = await _platformCompanyDbService.Get(userId);

            if (company == null)
                company = await _platformCompanyDbService.Add(new PlatformCompany { Id = userId });

            return new PlatformCompanyDto
            {
                About = company.About,
                BankIdentificationCode = company.BankIdentificationCode,
                CheckingAccount = company.CheckingAccount,
                CompanyGroup = company.CompanyGroup,
                CorrespondentAccount = company.CorrespondentAccount,
                Email = company.Email,
                LegalAddress = company.LegalAddress,
                MailingAddress = company.MailingAddress,
                Name = company.Name,
                PhoneNumber = company.PhoneNumber,
                Site = company.Site,
                TaxpayerNumber = company.TaxpayerNumber,
                OrganizationType = company.OrganizationType
            };
        }

        public async Task<ResponseData> Edit(int userId, PlatformCompanyEditDto request)
        {
            // Validations

            if (RegexValidator.ContainsLink(request.About))
                return ResponseData.Fail("About", "About contains links!");
            else if (RegexValidator.ContainsEmail(request.About))
                return ResponseData.Fail("About", "About contains emails!");
            else if (RegexValidator.ContainsContact(request.About))
                return ResponseData.Fail("About", "About contains contacts!");
            else if (RegexValidator.ContainsPhoneNumber(request.About))
                return ResponseData.Fail("About", "About contains phone numbers!");

            if (request.OrganizationType != PlatformCompanyOrganizationTypes.BUSINESS && request.OrganizationType != PlatformCompanyOrganizationTypes.ORGANIZATION && request.OrganizationType != PlatformCompanyOrganizationTypes.SOLETRADER)
            {
                return ResponseData.Fail("Incorrect", "OrganizationType!");
            }

            // End validation

            PlatformCompany company = await _platformCompanyDbService.Get(userId);

            company.About = request.About;
            company.BankIdentificationCode = request.BankIdentificationCode;
            company.CheckingAccount = request.CheckingAccount;
            company.CompanyGroup = request.CompanyGroup;
            company.CorrespondentAccount = request.CorrespondentAccount;
            company.Email = request.Email;
            company.LegalAddress = request.LegalAddress;
            company.MailingAddress = request.MailingAddress;
            company.Name = request.Name;
            company.PhoneNumber = request.PhoneNumber;
            company.Site = request.Site;
            company.TaxpayerNumber = request.TaxpayerNumber;
            company.OrganizationType = request.OrganizationType;

            await _platformCompanyDbService.Update(company);

            return new ResponseData();
        }
    }
}
