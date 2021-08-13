using diga.bl.Constants;
using diga.bl.Models.Platform.Notifications;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.web.Models.PlatformNotifications;
using diga.web.Models.UserRatings;
using diga.web.Services.PlatformNotificationServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformUserRatingServices
{
    public class PlatformUserRatingService : IPlatformUserRatingService
    {
        private readonly IPlatformUserRatingDbService _platformUserRatingDbService;
        private readonly IPlatformNotificationService _platformNotificationService;

        public PlatformUserRatingService(IPlatformUserRatingDbService platformUserRatingDbService,
            IPlatformNotificationService platformNotificationService)
        {
            _platformUserRatingDbService = platformUserRatingDbService;
            _platformNotificationService = platformNotificationService;
        }

        public async Task Add(PlatformUserRating platformUserRating)
        {
            await _platformUserRatingDbService.Add(platformUserRating);
            await _platformNotificationService.AddNotification(platformUserRating.ApplicationUserId,
                new PlatformNotificationDto
                {
                    IsRead = false,
                    Type = PlatformNotificationTypes.AddUserRating,
                    DateTime = platformUserRating.DateTime,
                    Datas = new List<PlatformNotificationDataDto>
                    {
                        new PlatformNotificationDataDto("Points", platformUserRating.Points.ToString())
                    }
                });
        }

        public async Task<UserRatingDto> Get(int userId)
        {
            return new UserRatingDto
            {
                Points = await _platformUserRatingDbService.CalculateRating(userId)
            };
        }
    }
}
