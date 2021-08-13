using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Information;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformChatRoomDbServices;
using diga.dal.DbServices.PlatformChatRoomMessageDbServices;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.dal.DbServices.PlatformNotificationDbServices;
using diga.dal.DbServices.PlatformTagDbServices;
using diga.dal.DbServices.PlatformUserCategoryDbServices;
using diga.dal.DbServices.PlatformUserLanguageDbServices;
using diga.dal.DbServices.PlatformUserPhoneNumberDbServices;
using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.dal.DbServices.PlatformUserTagDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.web.Helpers;
using diga.web.Models.Status;
using diga.web.Models.Users;
using diga.web.Services.PlatformUserRatingServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserDbService _userDbService;
        private readonly IPlatformUserRatingDbService _platformUserRatingDbService;
        private readonly IPlatformUserRatingService _platformUserRatingService;
        private readonly IPlatformUserPhoneNumberDbService _platformUserPhoneNumberDbService;
        private readonly IPlatformUserLanguageDbService _platformUserLanguageDbService;
        private readonly IPlatformUserCategoryDbService _platformUserCategoryDbService;
        private readonly IPlatformUserTagDbService _platformUserTagDbService;
        private readonly IPlatformTagDbService _platformTagDbService;
        private readonly IPlatformContractReviewDbService _platformContractReviewDbService;
        private readonly IPlatformChatRoomDbService _platformChatRoomDbService;
        private readonly IPlatformChatRoomMessageDbService _platformChatRoomMessageDbService;
        private readonly IPlatformNotificationDbService _platformNotificationDbService;
        private readonly IWebHostEnvironment _environment;
        private readonly PasswordHelper _passwordHelper;
        private readonly EmailHelper _emailHelper;

        public UserService(IUserDbService userDbService,
            IPlatformUserRatingDbService platformUserRatingDbService,
            IPlatformUserRatingService platformUserRatingService,
            IPlatformUserPhoneNumberDbService platformUserPhoneNumberDbService,
            IPlatformUserLanguageDbService platformUserLanguageDbService,
            IPlatformUserCategoryDbService platformUserCategoryDbService,
            IPlatformUserTagDbService platformUserTagDbService,
            IPlatformTagDbService platformTagDbService,
            IPlatformContractReviewDbService platformContractReviewDbService,
            IPlatformChatRoomDbService platformChatRoomDbService,
            IPlatformChatRoomMessageDbService platformChatRoomMessageDbService,
            IPlatformNotificationDbService platformNotificationDbService,
            IWebHostEnvironment environment,
            PasswordHelper passwordHelper,
            EmailHelper emailHelper)
        {
            _userDbService = userDbService;
            _platformUserRatingDbService = platformUserRatingDbService;
            _platformUserRatingService = platformUserRatingService;
            _platformUserPhoneNumberDbService = platformUserPhoneNumberDbService;
            _platformUserLanguageDbService = platformUserLanguageDbService;
            _platformUserCategoryDbService = platformUserCategoryDbService;
            _platformUserTagDbService = platformUserTagDbService;
            _platformTagDbService = platformTagDbService;
            _platformContractReviewDbService = platformContractReviewDbService;
            _platformChatRoomDbService = platformChatRoomDbService;
            _platformChatRoomMessageDbService = platformChatRoomMessageDbService;
            _platformNotificationDbService = platformNotificationDbService;
            _environment = environment;
            _emailHelper = emailHelper;

            _passwordHelper = passwordHelper;
        }

        #region General
        public async Task<ResponseData> Complaint(int userId, UserComplaintDto request)
        {
            var fromUser = await _userDbService.Get(userId);
            var toUser = await _userDbService.Get(request.UserId);

            await _emailHelper.SendAsync("geral@diga.pt", $"{fromUser.Name} made complaint to {toUser.Name}",
                $"<b>Who complained</b>: {fromUser.Name} ({fromUser.Email}) <br> <b>Whom was complained</b>: {toUser.Name} ({toUser.Email}) <br> <b>Complaint text</b>: {request.Text}");
            return new ResponseData();
        }
        public async Task<ResponseData> EditSettings(int userId, UserSettingsDto request)
        {
            ApplicationUser user = await _userDbService.GetProfile(userId);
            user.NewMessagesEmailNotification = request.NewMessagesEmailNotification;
            await _userDbService.Update(user);

            return new ResponseData();
        }
        public async Task<UserSettingsDto> GetSettings(int userId)
        {
            ApplicationUser user = await _userDbService.GetProfile(userId);
            return new UserSettingsDto
            {
                NewMessagesEmailNotification = user.NewMessagesEmailNotification
            };
        }

        public async Task<ResponseData> LastSeen(int userId, DateTime lastSeen)
        {
            ApplicationUser user = await _userDbService.GetProfile(userId);
            user.LastSeen = lastSeen;
            await _userDbService.Update(user);

            return new ResponseData();
        }

        public async Task<UserFullDto> Get(int id)
        {
            ApplicationUser user = await _userDbService.GetProfile(id);

            var roomIds = await _platformChatRoomDbService.ListIds(id);

            return new UserFullDto
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Education = user.Education,
                Email = user.Email,
                Language = user.Language,
                Name = user.Name,
                Surname = user.Surname,
                NationalityId = user.NationalityId,
                ResidenceCountryId = user.ResidenceCountryId,
                MainPhoneNumber = user.PhoneNumber,
                VerificationStatus = user.PlatformUserVerifications.OrderBy(v=>v.Created).FirstOrDefault()?.Status,
                PhoneNumbers = user.PhoneNumbers.Select(n => new UserPhoneNumberDto
                {
                    Id = n.Id,
                    Type = n.Type,
                    Value = n.Value
                }).ToList(),
                Languages = user.Languages.Select(l => new UserLanguageDto
                {
                    Id = l.PlatformLanguage.Id,
                    Name = l.PlatformLanguage.Name
                }).ToList(),
                PostalCode = user.PostalCode,
                Resume = user.Resume,
                IsCompany = user.IsCompany,
                UnreadMessagesCount = _platformChatRoomMessageDbService.UnreadCount(roomIds, id),
                UnreadNotificationsCount = await _platformNotificationDbService.CountUnread(id),

                IsPaymentUserCreated = !string.IsNullOrEmpty(user.MangoPayUserId),
                IsWalletCreated = !string.IsNullOrEmpty(user.MangoPayWalletId),
                IsBankAccountConnected = user.PlatformMangoPayUserBankAccounts.Count > 0,
                BankAccountValidationStatus = user.MangoPayKYCValidationStatus,
                Domain = string.IsNullOrEmpty(user.Domain) ? (string)null : $"https://{user.Domain}.diga.pt"
            };
        }

        public async Task<UserCardDto> GetCard(int id)
        {
            ApplicationUser user = await _userDbService.Get(id);
            (int, int) reviews = await _platformContractReviewDbService.Count(id);

            return new UserCardDto
            {
                Avatar = user.Avatar,
                Name = user.Name,
                FullName = $"{user.Name} {user.Surname}",
                Surname = user.Surname,
                Rating = await _platformUserRatingDbService.CalculateRating(id),
                GoodReviews = reviews.Item1,
                BadReviews = reviews.Item2
            };
        }

        public async Task<ResponseData> Edit(int id, UserEditDto request)
        {
            // Validations

            if (RegexValidator.ContainsLink(request.Resume))
                return ResponseData.Fail("Resume", "Resume contains links!");
            else if (RegexValidator.ContainsEmail(request.Resume))
                return ResponseData.Fail("Resume", "Resume contains emails!");
            else if (RegexValidator.ContainsContact(request.Resume))
                return ResponseData.Fail("Resume", "Resume contains contacts!");
            else if (RegexValidator.ContainsPhoneNumber(request.Resume))
                return ResponseData.Fail("Resume", "Resume contains phone numbers!");

            // End validation

            ApplicationUser user = await _userDbService.GetProfile(id);

            if (user.Avatar != request.Avatar)
            {
                if (!string.IsNullOrEmpty(request.Avatar))
                {
                    string tempPath = Path.Combine(_environment.WebRootPath, "img", "temp", request.Avatar);
                    string newPath = Path.Combine(_environment.WebRootPath, "img", "src", request.Avatar);

                    if (File.Exists(tempPath))
                    {
                        if (!string.IsNullOrEmpty(user.Avatar))
                        {
                            string existAvatar = Path.Combine(_environment.WebRootPath, "img", "src", user.Avatar);

                            if (File.Exists(existAvatar))
                                File.Delete(existAvatar);
                        }

                        File.Move(tempPath, newPath);
                        user.Avatar = request.Avatar;

                        if (!await _platformUserRatingDbService.Any(user.Id, PlatformUserRatingsDescriptions.Avatar))
                        {
                            await _platformUserRatingService.Add(new PlatformUserRating
                            {
                                ApplicationUserId = user.Id,
                                Description = PlatformUserRatingsDescriptions.Avatar,
                                DateTime = DateTime.UtcNow,
                                Points = 3
                            });
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.Avatar))
                    {
                        string existAvatar = Path.Combine(_environment.WebRootPath, "img", "src", user.Avatar);

                        if (File.Exists(existAvatar))
                            File.Delete(existAvatar);
                    }

                    user.Avatar = null;
                }
            }

            user.Address = request.Address;
            user.DateOfBirth = request.DateOfBirth;
            user.Education = request.Education;
            user.Email = request.Email;
            user.Language = request.Language;
            user.Name = request.Name;
            user.Surname = request.Surname;
            if (request.NationalityId > 0) { user.NationalityId = request.NationalityId; }
            if (request.ResidenceCountryId > 0) { user.ResidenceCountryId = request.ResidenceCountryId; }


            user.PostalCode = request.PostalCode;
            user.Resume = request.Resume;
            user.IsCompany = request.IsCompany;
            user.PhoneNumber = request.MainPhoneNumber;

            await _userDbService.Update(user);

            // Update numbers
            List<PlatformUserPhoneNumber> removeNumbers =
                user.PhoneNumbers.Where(un => !request.PhoneNumbers.Any(n => un.Id == n.Id)).ToList();
            List<PlatformUserPhoneNumber> addNumbers =
                request.PhoneNumbers.Where(n => !user.PhoneNumbers.Any(un => un.Type == n.Type && un.Value == n.Value))
                .Select(p => new PlatformUserPhoneNumber { ApplicationUserId = user.Id, Type = p.Type, Value = p.Value }).ToList();

            await _platformUserPhoneNumberDbService.RemoveRange(removeNumbers);
            await _platformUserPhoneNumberDbService.AddRange(addNumbers);

            // Update languages
            await _platformUserLanguageDbService.RemoveRange(user.Languages.Where(l => !request.LanguageIds.Contains(l.PlatformLanguageId)));
            await _platformUserLanguageDbService.AddRange(request.LanguageIds.Where(l => !user.Languages.Any(ul => ul.PlatformLanguageId == l))
                .Select(l => new PlatformUserLanguage { ApplicationUserId = user.Id, PlatformLanguageId = l }));

            // Add rating
            if (!string.IsNullOrEmpty(user.Address) && user.DateOfBirth.HasValue
                && !string.IsNullOrEmpty(user.Education) && !string.IsNullOrEmpty(user.Email)
                && !string.IsNullOrEmpty(user.Language) && !string.IsNullOrEmpty(user.Name)
                && user.NationalityId.HasValue && !string.IsNullOrEmpty(user.PostalCode)
                && !string.IsNullOrEmpty(user.Resume) && !string.IsNullOrEmpty(user.Resume)
                && !string.IsNullOrEmpty(user.PhoneNumber) && request.LanguageIds.Count != 0
                && !await _platformUserRatingDbService.Any(user.Id, PlatformUserRatingsDescriptions.UserInfo))
            {
                await _platformUserRatingService.Add(new PlatformUserRating
                {
                    ApplicationUserId = user.Id,
                    Description = PlatformUserRatingsDescriptions.UserInfo,
                    DateTime = DateTime.UtcNow,
                    Points = 6
                });
            }

            return new ResponseData();
        }

        public async Task<ResponseData> ChangePassword(int userId, PasswordChangeDto request)
        {
            ApplicationUser user = await _userDbService.Get(userId);

            if (!_passwordHelper.Compare(request.OldPassword, user.PasswordHash))
                return ResponseData.Fail("OldPassword", "Incorrect password!");

            if (request.NewPassword.Length < 6)
                return ResponseData.Fail("NewPassword", "Password must be at least 6 characters long!");

            if (request.NewPassword.Length > 12)
                return ResponseData.Fail("NewPassword", "Password must be 12 characters maximum!");

            user.PasswordCode = RandomHelper.GetRandomString(16);
            user.PasswordHash = _passwordHelper.Generate(request.NewPassword);
            await _userDbService.Update(user);

            return new ResponseData();
        }

        #endregion

        #region Categories

        public async Task<ResponseData> AddCategory(int userId, UserCategoryAddDto request)
        {
            if (!await _platformUserCategoryDbService.Any(userId, request.CategoryId))
            {
                await _platformUserCategoryDbService.Add(new PlatformUserCategory { ApplicationUserId = userId, PlatformCategoryId = request.CategoryId });
            }

            return new ResponseData();
        }

        public async Task<ResponseData> RemoveCategory(int userId, int categoryId)
        {
            if (await _platformUserCategoryDbService.Any(userId, categoryId))
            {
                PlatformUserCategory category = await _platformUserCategoryDbService.Get(userId, categoryId);
                await _platformUserCategoryDbService.Remove(category);
            }

            return new ResponseData();
        }

        public async Task<List<UserCategoryDto>> ListCategories(int userId)
        {
            return (await _platformUserCategoryDbService.List(userId))
                .Select(c => new UserCategoryDto
                {
                    Id = c.PlatformCategoryId,
                    Name = c.PlatformCategory.NameId,
                    ParentCategoryId = c.PlatformCategory.ParentCategoryId
                }).ToList();
        }

        #endregion

        #region Tags

        public async Task<ResponseData> AddTag(int userId, UserTagAddDto request)
        {
            if (await _platformUserTagDbService.Count(userId) >= 20)
                return ResponseData.Fail("Count", "Count over than 20!");

            if (!await _platformTagDbService.Any(request.Tag))
                await _platformTagDbService.Add(new PlatformTag { Name = request.Tag });

            if (!await _platformUserTagDbService.Any(userId, request.Tag))
            {
                PlatformTag tag = await _platformTagDbService.Get(request.Tag);
                await _platformUserTagDbService.Add(new PlatformUserTag { ApplicationUserId = userId, PlatformTagId = tag.Id });
            }

            return new ResponseData();
        }

        public async Task<ResponseData> RemoveTag(int userId, string tag)
        {
            if (await _platformUserTagDbService.Any(userId, tag))
            {
                PlatformTag platformTag = await _platformTagDbService.Get(tag);
                PlatformUserTag userTag = await _platformUserTagDbService.Get(userId, platformTag.Id);
                await _platformUserTagDbService.Remove(userTag);
            }

            return new ResponseData();
        }

        public async Task<List<UserTagDto>> ListTags(int userId)
        {
            return (await _platformUserTagDbService.List(userId))
                .Select(c => new UserTagDto
                {
                    Id = c.PlatformTagId,
                    Name = c.PlatformTag.Name
                }).ToList();
        }

        #endregion
    }
}
