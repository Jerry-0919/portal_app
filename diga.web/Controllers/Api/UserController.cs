using diga.web.Models.Status;
using diga.web.Models.Users;
using diga.web.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            : base(null, null)
        {
            _userService = userService;
        }

        [HttpGet("info")]
        public async Task<UserFullDto> GetInfo()
        {
            return await _userService.Get(UserId);
        }

        [HttpGet("card")]
        public async Task<UserCardDto> GetCard()
        {
            return await _userService.GetCard(UserId);
        }

        [HttpGet("card/{userId}")]
        public async Task<UserCardDto> GetCard(int userId)
        {
            return await _userService.GetCard(userId);
        }

        [HttpPut("edit")]
        public async Task<ResponseData> Edit(UserEditDto request)
        {
            return await _userService.Edit(UserId, request);
        }

        [HttpPost("password/change")]
        public async Task<ResponseData> ChangePassword(PasswordChangeDto request)
        {
            return await _userService.ChangePassword(UserId, request);
        }

        [HttpGet("categories/list")]
        public async Task<List<UserCategoryDto>> ListCategories()
        {
            return await _userService.ListCategories(UserId);
        }

        [HttpPost("categories/add")]
        public async Task<ResponseData> AddCategory(UserCategoryAddDto request)
        {
            return await _userService.AddCategory(UserId, request);
        }

        [HttpDelete("categories/remove/{categoryId}")]
        public async Task<ResponseData> RemoveCategory(int categoryId)
        {
            return await _userService.RemoveCategory(UserId, categoryId);
        }

        [HttpGet("tags/list")]
        public async Task<List<UserTagDto>> ListTags()
        {
            return await _userService.ListTags(UserId);
        }

        [HttpPost("tags/add")]
        public async Task<ResponseData> AddTag(UserTagAddDto request)
        {
            return await _userService.AddTag(UserId, request);
        }

        [HttpDelete("tags/remove/{tag}")]
        public async Task<ResponseData> RemoveTag(string tag)
        {
            return await _userService.RemoveTag(UserId, tag);
        }

        [HttpGet("settings")]
        public async Task<UserSettingsDto> Settings()
        {
            return await _userService.GetSettings(UserId);
        }

        [HttpPut("settings")]
        public async Task<ResponseData> SettingsEdit(UserSettingsDto request)
        {
            return await _userService.EditSettings(UserId, request);
        }

        [HttpPost("complaint")]
        public async Task<ResponseData> Complaint(UserComplaintDto request)
        {
            return await _userService.Complaint(UserId, request);
        }
    }
}
