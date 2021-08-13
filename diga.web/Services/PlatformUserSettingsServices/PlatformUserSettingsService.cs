using diga.bl.Models.Platform.Information;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformCategoryDbServices;
using diga.dal.DbServices.PlatformUserFilterCategoryDbServices;
using diga.dal.DbServices.PlatformUserSettingsDbServices;
using diga.web.Models.PlatformUserSettings;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformUserSettingsServices
{
    public class PlatformUserSettingsService : IPlatformUserSettingsService
    {
        private readonly IPlatformUserSettingsDbService _platformUserSettingsDbService;
        private readonly IPlatformCategoryDbService _platformCategoryDbService;
        private readonly IPlatformUserFilterCategoryDbService _platformUserFilterCategoryDbService;

        public PlatformUserSettingsService(IPlatformUserSettingsDbService platformUserSettingsDbService,
            IPlatformCategoryDbService platformCategoryDbService,
            IPlatformUserFilterCategoryDbService platformUserFilterCategoryDbService)
        {
            _platformUserSettingsDbService = platformUserSettingsDbService;
            _platformCategoryDbService = platformCategoryDbService;
            _platformUserFilterCategoryDbService = platformUserFilterCategoryDbService;
        }

        public async Task<ResponseData> UpdateFilter(int userId, PlatformUserFilterSettingsDto settings)
        {
            PlatformUserSettings userSettings = await _platformUserSettingsDbService.GetFull(userId);

            if (userSettings == null)
            {
                List<PlatformCategory> categories = await _platformCategoryDbService.List(settings.Categories);
                await _platformUserSettingsDbService.Add(new PlatformUserSettings
                {
                    Id = userId,
                    FilterHideMyBids = settings.HideMyBids,
                    FilterCategories = categories.Select(c => new PlatformUserFilterCategory
                    {
                        PlatformCategoryId = c.Id
                    }).ToList()
                });
            }
            else
            {
                userSettings.FilterHideMyBids = settings.HideMyBids;

                List<PlatformUserFilterCategory> categoriesToRemove = userSettings.FilterCategories
                    .Where(fc => !settings.Categories.Any(c => fc.PlatformCategoryId == c)).ToList();

                List<PlatformUserFilterCategory> categoriesToAdd = settings.Categories
                    .Where(c => !userSettings.FilterCategories.Any(fc => fc.PlatformCategoryId == c))
                    .Select(c => new PlatformUserFilterCategory
                    {
                        PlatformCategoryId = c,
                        PlatformUserSettingsId = userSettings.Id
                    }).ToList();

                await _platformUserSettingsDbService.Update(userSettings);
                await _platformUserFilterCategoryDbService.RemoveRange(categoriesToRemove);
                await _platformUserFilterCategoryDbService.AddRange(categoriesToAdd);
            }

            return new ResponseData();
        }

        public async Task<PlatformUserFilterSettingsDto> GetFilter(int userId)
        {
            PlatformUserSettings settings = await _platformUserSettingsDbService.GetFull(userId);

            if (settings == null)
                return new PlatformUserFilterSettingsDto { };

            return new PlatformUserFilterSettingsDto
            {
                HideMyBids = settings.FilterHideMyBids,
                Categories = settings.FilterCategories.Select(c => c.PlatformCategoryId).ToList()
            };
        }
    }
}
