using diga.bl.Models.Platform.Users;
using diga.web.Models.UserRatings;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformUserRatingServices
{
    public interface IPlatformUserRatingService
    {
        Task<UserRatingDto> Get(int userId);
        Task Add(PlatformUserRating platformUserRating);
    }
}