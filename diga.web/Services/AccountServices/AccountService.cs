using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.dal.DbServices.RoleDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.dal.DbServices.UserTariffDbServices;
using diga.dal.DbServices.TariffDbServices;
using diga.web.Extensions;
using diga.web.Helpers;
using diga.web.Models.Account;
using diga.web.Models.Status;
using diga.web.Options;
using diga.web.Services.PlatformUserRatingServices;
using diga.web.Validators.Account;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace diga.web.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly IUserDbService _accountDbService;
        private readonly IRoleDbService _roleDbService;
        private readonly IPlatformUserRatingService _platformUserRatingService;
        private readonly IUserTariffDbService _userTariffService;
        private readonly ITariffDbService _tariffService;

        private readonly DomainOptions _domainOptions;

        private readonly SignUpRequestValidator _signUpRequestValidator;
        private readonly PasswordHelper _passwordHelper;
        private readonly EmailHelper _emailHelper;
        private readonly SaasHelper _saasHelper;

        public AccountService(IUserDbService accountDbService,
            IRoleDbService roleDbService,
            IPlatformUserRatingService platformUserRatingService,
            IUserTariffDbService userTariffService,
            ITariffDbService tariffService,
            IOptions<DomainOptions> domainOptions,
            SignUpRequestValidator signUpRequestValidator,
            PasswordHelper passwordHelper,
            EmailHelper emailHelper,
            SaasHelper saasHelper)
        {
            _accountDbService = accountDbService;
            _roleDbService = roleDbService;
            _platformUserRatingService = platformUserRatingService;
            _userTariffService = userTariffService;
            _tariffService = tariffService;

            _domainOptions = domainOptions.Value;

            _signUpRequestValidator = signUpRequestValidator;
            _passwordHelper = passwordHelper;
            _emailHelper = emailHelper;
            _saasHelper = saasHelper;
        }

        public async Task<ResponseData<SignUpResponseDto>> SignUp(SignUpRequestDto request)
        {
            ResponseData<SignUpResponseDto> response = new ResponseData<SignUpResponseDto> { };

            ValidationResult validationResult = _signUpRequestValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                response.Errors = validationResult.ToErrors();
                return response;
            }

            if (await _accountDbService.Any(request.Email))
            {
                response.AddError("Email", "User with this email is already registered!");
                return response;
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                AuthToken = Guid.NewGuid().ToString(),
                Language = request.Language,
                Balance = 0,
                UserRoles = new List<ApplicationUserRole> { }
            };

            PasswordHelper passwordHelper = new PasswordHelper();
            user.PasswordHash = passwordHelper.Generate(request.Password);

            ApplicationRole userRole = await _roleDbService.Get("Erp");
            user.UserRoles.Add(new ApplicationUserRole { RoleId = userRole.Id });

            if (!string.IsNullOrEmpty(request.Domain))
            {
                user.Domain = request.Domain;
                var tariff = await _tariffService.Get("base");
                user.UserTariffs.Add(new UserTariffs
                {
                    TariffId = tariff.Id,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddYears(5),
                    IsTrial = false,
                    CurrentPrice = tariff.Price,
                    PriceForNextUser = tariff.PriceForNextUser,
                    NumberOfUsers = tariff.NumberOfUsers
                });
                if (await _saasHelper.CreateInstance(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.PhoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("_", ""),
                    request.Domain,
                    request.Language,
                    "",
                    tariff.NumberOfUsers,
                    "base",
                    tariff.Days.Value,
                    0,
                    string.Join(",", tariff.TariffModules.Select(tm => tm.Module.Name).ToList()),
                    user.AuthToken
                ) == false)
                {
                    response.AddError("Erp saas", "Problem with generating new ERP");
                    return response;
                }
            }

            await _accountDbService.Add(user);
            user = await _accountDbService.GetFull(request.Email);

            // Add rating
            await _platformUserRatingService.Add(new PlatformUserRating
            {
                ApplicationUserId = user.Id,
                Description = PlatformUserRatingsDescriptions.SignUp,
                DateTime = DateTime.UtcNow,
                Points = 5
            });

            return new ResponseData<SignUpResponseDto>
            {
                Data = new SignUpResponseDto
                {
                    Token = new JwtHelper().GetToken(new ClaimsHelper().CommonClaims(user)),
                    Username = user.Email,
                    Name = user.Name
                }
            };
        }

        public async Task<ResponseData> ForgotPassword(PasswordForgotRequestDto request)
        {
            ApplicationUser user = await _accountDbService.Get(request.Email);

            user.PasswordCode = RandomHelper.GetRandomString(16);
            await _accountDbService.Update(user);

            await _emailHelper.SendAsync(user.Email, "Password recovery",
                $"Recovery link: {_domainOptions.Client}/platform/index#/set-new-password/{user.PasswordCode}");

            return new ResponseData();
        }

        public async Task<ResponseData> CheckForgotPassword(PasswordForgotCheckRequestDto request)
        {
            ApplicationUser user = await _accountDbService.GetByPasswordCode(request.Code);

            if (request.NewPassword.Length < 6)
                return ResponseData.Fail("NewPassword", "Password must be at least 6 characters long!");

            if (request.NewPassword.Length > 12)
                return ResponseData.Fail("NewPassword", "Password must be 12 characters maximum!");

            if (user.PasswordCode != request.Code)
                return ResponseData.Fail("Code", "Incorrect code!");

            user.PasswordHash = _passwordHelper.Generate(request.NewPassword);
            await _accountDbService.Update(user);

            return new ResponseData();
        }
    }
}
