using diga.dal.DbServices.UserDbServices;
using diga.web.Models.Status;
using diga.web.Options;
using MangoPay.SDK;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangoPay.SDK.Core.Enumerations;
using diga.dal.DbServices.PlatformCompanyDbServices;
using diga.bl.Models;
using diga.bl.Models.Platform.Users;
using MangoPay.SDK.Entities.POST;
using diga.dal.DbServices.PlatformMangoPayUserBankAccountDbServices;
using diga.web.Models.PlatformUserKYC;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using diga.dal.DbServices.PlatformMangoPayUserKYCDocumentDbService;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformPayServices;
using MangoPay.SDK.Entities.GET;
using diga.bl.Constants;

namespace diga.web.Services.MangoPayServices
{
    public class MangoPayUserService : IMangoPayUserService
    {
        private readonly IUserDbService _userDbService;
        private readonly IPlatformCompanyDbService _platformCompanyDbService;
        private readonly IPlatformMangoPayUserBankAccountDbService _platformMangoPayUserBankAccountDbService;
        private readonly IPlatformMangoPayUserKYCDocumentDbService _platformMangoPayUserKYCDocumentDbService;
        private readonly MangoPayOptions _mangoPayOptions;
        private readonly MangoPayApi _api;
        private readonly IWebHostEnvironment _environment;

        public MangoPayUserService(IOptions<MangoPayOptions> mangoPayOptions,
            IUserDbService userDbService,
            IPlatformCompanyDbService platformCompanyDbService,
            IPlatformMangoPayUserBankAccountDbService platformMangoPayUserBankAccountDbService,
            IPlatformMangoPayUserKYCDocumentDbService platformMangoPayUserKYCDocumentDbService,
            IWebHostEnvironment environment)
        {
            _userDbService = userDbService;
            _platformCompanyDbService = platformCompanyDbService;
            _platformMangoPayUserBankAccountDbService = platformMangoPayUserBankAccountDbService;
            _platformMangoPayUserKYCDocumentDbService = platformMangoPayUserKYCDocumentDbService;
            _mangoPayOptions = mangoPayOptions.Value;
            _environment = environment;

            _api = new MangoPayApi();
            _api.Config.ClientId = _mangoPayOptions.ClientId;
            _api.Config.ClientPassword = _mangoPayOptions.ClientPassword;
            _api.Config.BaseUrl = _mangoPayOptions.BaseUrl;
        }

        private async Task<Tuple<ResponseData, string>> CreateUser(int userId)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            var platformCompany = await _platformCompanyDbService.Get(userId);

            if (applicationUser.Nationality == null || (applicationUser.Nationality != null && string.IsNullOrEmpty(applicationUser.Nationality.Code)))
            {
                return new Tuple<ResponseData, string>(ResponseData.Fail("Not enough information", "Nationality"), "");
            }
            if (applicationUser.ResidenceCountry == null || (applicationUser.ResidenceCountry != null && string.IsNullOrEmpty(applicationUser.ResidenceCountry.Code)))
            {
                return new Tuple<ResponseData, string>(ResponseData.Fail("Not enough information", "ResidenceCountry"), "");
            }
            if (applicationUser.DateOfBirth == null)
            {
                return new Tuple<ResponseData, string>(ResponseData.Fail("Not enough information", "DateOfBirth"), "");
            }

            if (platformCompany != null && !string.IsNullOrEmpty(platformCompany.Name))
            {
                if (string.IsNullOrEmpty(platformCompany.OrganizationType))
                {
                    return new Tuple<ResponseData, string>(ResponseData.Fail("Not enough information", "OrganizationType"), "");
                }
                var company = await _api.Users.CreateAsync(new MangoPay.SDK.Entities.POST.UserLegalPostDTO(
                    applicationUser.Email,
                    platformCompany.Name,
                    (LegalPersonType)Enum.Parse(typeof(LegalPersonType), platformCompany.OrganizationType),
                    applicationUser.Name,
                    applicationUser.Surname,
                    applicationUser.DateOfBirth.Value,
                    (CountryIso)Enum.Parse(typeof(CountryIso), applicationUser.Nationality.Code),
                    (CountryIso)Enum.Parse(typeof(CountryIso), applicationUser.ResidenceCountry.Code)
                    ));
                applicationUser.MangoPayUserId = company.Id;
                await _userDbService.Update(applicationUser);
                return new Tuple<ResponseData, string>(new ResponseData(), company.Id);
            }
            else
            {
                var user = await _api.Users.CreateAsync(new MangoPay.SDK.Entities.POST.UserNaturalPostDTO(
                    applicationUser.Email,
                    applicationUser.Name,
                    applicationUser.Surname,
                    applicationUser.DateOfBirth.Value,
                    (CountryIso)Enum.Parse(typeof(CountryIso), applicationUser.Nationality.Code),
                    (CountryIso)Enum.Parse(typeof(CountryIso), applicationUser.ResidenceCountry.Code)
                    ));
                applicationUser.MangoPayUserId = user.Id;
                await _userDbService.Update(applicationUser);
                return new Tuple<ResponseData, string>(new ResponseData(), user.Id);
            }
        }

        public async Task<ResponseData> CreateWallet(int userId)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId))
            {
                var createUser = await CreateUser(userId);
                if (createUser.Item1.Errors.Count > 0)
                {
                    return createUser.Item1;
                }
                else
                {
                    applicationUser.MangoPayUserId = createUser.Item2;
                }
            }
            if (!string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                ResponseData.Fail("Fail", "Wallet already exists");
            }
            var wallet = await _api.Wallets.CreateAsync(new MangoPay.SDK.Entities.POST.WalletPostDTO(
                new List<string>() { applicationUser.MangoPayUserId },
                $"{applicationUser.Name}'s wallet",
                CurrencyIso.EUR)
                );
            applicationUser.MangoPayWalletId = wallet.Id;
            await _userDbService.Update(applicationUser);
            return ResponseData.Ok();
        }

        public async Task<PaginatedList<CardDto>> Cards(int userId, int page, int perPage)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                throw new System.Exception("Payment user id or wallet id not registered");
            }
            var cards = await _api.Users.GetCardsAsync(applicationUser.MangoPayUserId, new MangoPay.SDK.Entities.Pagination(page, perPage));

            return new PaginatedList<CardDto>
            {
                CountAll = cards.TotalItems,
                List = cards.Select(c => new CardDto
                {
                    Id = c.Id,
                    Alias = c.Alias,
                    IsValid = c.Validity == Validity.VALID
                }).ToList()
            };
        }

        public async Task<CardRegistrationDTO> RegisterCard(int userId)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                throw new System.Exception("Payment user id or wallet id not registered");
            }

            return await _api.CardRegistrations.CreateAsync(new MangoPay.SDK.Entities.POST.CardRegistrationPostDTO(
                applicationUser.MangoPayUserId, CurrencyIso.EUR, CardType.CB_VISA_MASTERCARD));
        }

        public async Task<ResponseData> UpdateCardRegistration(int userId, string registrationId, CardRegistrationEditDto request)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                return ResponseData.Fail("User", "Payment user id or wallet id not registered");
            }

            await _api.CardRegistrations.UpdateAsync(new MangoPay.SDK.Entities.PUT.CardRegistrationPutDTO
            {
                RegistrationData = request.Token
            }, registrationId);

            return ResponseData.Ok();
        }

        public async Task<ResponseData> RegisterBankAccount(int userId, BankAccountIbanPostDTO request)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId))
            {
                return ResponseData.Fail("Fail", "Firtsly create user in payment system");
            }
            if (string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                return ResponseData.Fail("Fail", "Firtsly create wallet in payment system");
            }

            var account = await _api.Users.CreateBankAccountIbanAsync(applicationUser.MangoPayUserId, request);
            await _platformMangoPayUserBankAccountDbService.Add(new bl.Models.Platform.MangoPay.PlatformMangoPayUserBankAccount {
                ApplicationUserId = userId,
                MangoPayId = account.Id,
                Tag = request.Tag
            });

            return ResponseData.Ok();
        }

        public async Task<ResponseData> CreateKYCCompany(int userId, PlatformUserKYCCompanyDto request)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId))
            {
                return ResponseData.Fail("Fail", "Firtsly create user in payment system");
            }
            if (string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                return ResponseData.Fail("Fail", "Firtsly create wallet in payment system");
            }
            if (applicationUser.PlatformMangoPayUserBankAccounts.Count == 0)
            {
                return ResponseData.Fail("Fail", "Firtsly register bank account");
            }

            var document = await _api.Users.CreateKycDocumentAsync(applicationUser.MangoPayUserId, KycDocumentType.IDENTITY_PROOF);

            foreach (var doc in request.Documents)
            {
                string temp = Path.Combine(_environment.WebRootPath, "docs", "temp", doc.File);
                string src = Path.Combine(_environment.WebRootPath, "docs", "src", $"{doc.File}");

                File.Move(temp, src);

                await _api.Users.CreateKycPageAsync(applicationUser.MangoPayUserId, document.Id, File.ReadAllBytes(src));

                await _platformMangoPayUserKYCDocumentDbService.Add(new bl.Models.Platform.MangoPay.PlatformMangoPayUserKYCDocument
                {
                    ApplicationUserId = userId,
                    File = doc.File,
                    FileName = doc.FileName,
                    MangoPayId = document.Id
                });
            }

            await _api.Users.UpdateKycDocumentAsync(applicationUser.MangoPayUserId, new MangoPay.SDK.Entities.PUT.KycDocumentPutDTO
            {
                Status = KycStatus.VALIDATION_ASKED
            }, document.Id);

            applicationUser.MangoPayKYCValidationStatus = MangoPayKYCValidationStatuses.VALIDATION_ASKED;
            await _userDbService.Update(applicationUser);

            return ResponseData.Ok();
        }

        public async Task<ResponseData> CreateKYCIndividual(int userId, PlatformUserKYCIndividualDto request)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId))
            {
                return ResponseData.Fail("Fail", "Firtsly create user in payment system");
            }
            if (string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                return ResponseData.Fail("Fail", "Firtsly create wallet in payment system");
            }
            if (applicationUser.PlatformMangoPayUserBankAccounts.Count == 0)
            {
                return ResponseData.Fail("Fail", "Firtsly register bank account");
            }

            string temp = Path.Combine(_environment.WebRootPath, "docs", "temp", request.File);
            string src = Path.Combine(_environment.WebRootPath, "docs", "src", $"{request.File}");
            File.Move(temp, src);

            var document = await _api.Users.CreateKycDocumentAsync(applicationUser.MangoPayUserId, KycDocumentType.IDENTITY_PROOF);
            await _api.Users.CreateKycPageAsync(applicationUser.MangoPayUserId, document.Id, File.ReadAllBytes(src));
            await _api.Users.UpdateKycDocumentAsync(applicationUser.MangoPayUserId, new MangoPay.SDK.Entities.PUT.KycDocumentPutDTO
            {
                Status = KycStatus.VALIDATION_ASKED
            }, document.Id);

            await _platformMangoPayUserKYCDocumentDbService.Add(new bl.Models.Platform.MangoPay.PlatformMangoPayUserKYCDocument
            {
                ApplicationUserId = userId,
                File = request.File,
                FileName = request.FileName,
                MangoPayId = document.Id
            });

            applicationUser.MangoPayKYCValidationStatus = MangoPayKYCValidationStatuses.VALIDATION_ASKED;
            await _userDbService.Update(applicationUser);

            return ResponseData.Ok();
        }

        public async Task<PaginatedList<BankAccountDto>> BankAccounts(int userId, int page, int perPage)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                throw new System.Exception("Payment user id or wallet id not registered");
            }
            var accounts = await _api.Users.GetBankAccountsAsync(applicationUser.MangoPayUserId, new MangoPay.SDK.Entities.Pagination(page, perPage));

            return new PaginatedList<BankAccountDto>
            {
                CountAll = accounts.TotalItems,
                List = accounts.Select(c => new BankAccountDto
                {
                    Id = c.Id,
                    Active = c.Active,
                    OwnerAddressObsolete = c.OwnerAddressObsolete,
                    OwnerName = c.OwnerName
                }).ToList()
            };
        }
    }
}
