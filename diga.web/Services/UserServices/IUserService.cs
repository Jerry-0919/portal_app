using diga.web.Models.Status;
using diga.web.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.UserServices
{
    public interface IUserService
    {
        #region General
        Task<UserFullDto> Get(int userId);
        Task<UserCardDto> GetCard(int userId);
        Task<ResponseData> Edit(int userId, UserEditDto request);
        Task<ResponseData> LastSeen(int userId, DateTime lastSeen);
        Task<ResponseData> ChangePassword(int userId, PasswordChangeDto request);
        Task<UserSettingsDto> GetSettings(int userId);
        Task<ResponseData> EditSettings(int userId, UserSettingsDto request);
        Task<ResponseData> Complaint(int userId, UserComplaintDto request);
        #endregion

        #region Categories
        Task<ResponseData> AddCategory(int userId, UserCategoryAddDto request);
        Task<ResponseData> RemoveCategory(int userId, int categoryId);
        Task<List<UserCategoryDto>> ListCategories(int userId);
        #endregion

        #region Tags
        Task<ResponseData> AddTag(int userId, UserTagAddDto request);
        Task<ResponseData> RemoveTag(int userId, string tag);
        Task<List<UserTagDto>> ListTags(int userId);
        #endregion
    }
}
