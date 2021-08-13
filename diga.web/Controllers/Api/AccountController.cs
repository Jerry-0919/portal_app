using diga.bl.Models;
using diga.dal;
using diga.web.Helpers;
using diga.web.Models.Account;
using diga.web.Models.Status;
using diga.web.Services.AccountServices;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IConfiguration config, DigaContext _db,
            IAccountService accountService) : base(_db, config)
        {
            _accountService = accountService;
        }

        [HttpPost("token")]
        public async Task Token(LoginModel model)
        {
            var identity = GetIdentity(model.Email, model.Password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Wrong email or password");
                return;
            }

            var jwtHelper = new JwtHelper();
            var response = jwtHelper.CommonJwt(identity);

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost("/authToken")]
        public async Task AuthToken(AuthTokenModel model)
        {
            var authToken = model.AuthToken;

            var identity = GetIdentity(authToken);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Wrong email or password");
                return;
            }

            var jwtHelper = new JwtHelper();
            var response = jwtHelper.CommonJwt(identity);

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignUpResponseDto))]
        public async Task<ResponseData<SignUpResponseDto>> SignUp(SignUpRequestDto request)
        {
            return await _accountService.SignUp(request);
        }

        [HttpPost("password/forgot")]
        public async Task<ResponseData> ForgotPassword(PasswordForgotRequestDto request)
        {
            return await _accountService.ForgotPassword(request);
        }

        [HttpPost("password/forgot/check")]
        public async Task<ResponseData> CheckChangePassword(PasswordForgotCheckRequestDto request)
        {
            return await _accountService.CheckForgotPassword(request);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            ApplicationUser user = _db.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Email == username);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var passwordHelper = new PasswordHelper();

            if (passwordHelper.Compare(password, user.PasswordHash) == false)
            {
                throw new UnauthorizedAccessException();
            }

            var claimsHelper = new ClaimsHelper();
            return claimsHelper.CommonClaims(user);
        }

        private ClaimsIdentity GetIdentity(string authToken)
        {
            ApplicationUser user = _db.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.AuthToken == authToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var claimsHelper = new ClaimsHelper();
            return claimsHelper.CommonClaims(user);
        }
    }
}