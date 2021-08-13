using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformCategoryDbServices;
using diga.dal.DbServices.PlatformCompanyDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.dal.DbServices.PlatformPortfolioAlbumDbServices;
using diga.dal.DbServices.PlatformPortfolioVideoDbServices;
using diga.dal.DbServices.PlatformUserCategoryDbServices;
using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.web.Models.PlatformPublicProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPublicProfile
{
    public class PlatformPublicProfileService : IPlatformPublicProfileService
    {
        private readonly IUserDbService _userDbService;
        private readonly IPlatformContractReviewDbService _platformContractReviewDbService;
        private readonly IPlatformUserRatingDbService _platformUserRatingDbService;
        private readonly IPlatformUserCategoryDbService _platformUserCategoryDbService;
        private readonly IPlatformPortfolioAlbumDbService _platformPortfolioAlbumDbService;
        private readonly IPlatformPortfolioVideoDbService _platformPortfolioVideoDbService;
        private readonly IPlatformCompanyDbService _platformCompanyDbService;
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformCategoryDbService _platformCategoryDbService;

        public PlatformPublicProfileService(IUserDbService userDbService,
             IPlatformContractReviewDbService platformContractReviewDbService,
             IPlatformUserRatingDbService platformUserRatingDbService,
             IPlatformUserCategoryDbService platformUserCategoryDbService,
             IPlatformPortfolioAlbumDbService platformPortfolioAlbumDbService,
             IPlatformPortfolioVideoDbService platformPortfolioVideoDbService,
             IPlatformCompanyDbService platformCompanyDbService,
             IPlatformContractDbService platformContractDbService,
             IPlatformCategoryDbService platformCategoryDbService)
        {
            _userDbService = userDbService;
            _platformContractReviewDbService = platformContractReviewDbService;
            _platformUserRatingDbService = platformUserRatingDbService;
            _platformUserCategoryDbService = platformUserCategoryDbService;
            _platformPortfolioAlbumDbService = platformPortfolioAlbumDbService;
            _platformPortfolioVideoDbService = platformPortfolioVideoDbService;
            _platformCompanyDbService = platformCompanyDbService;
            _platformContractDbService = platformContractDbService;
            _platformCategoryDbService = platformCategoryDbService;
        }

        public async Task<PlatformPublicProfileDto> Get(int userId)
        {
            var user = await _userDbService.GetFull(userId);
            (int, int) reviews = await _platformContractReviewDbService.Count(userId);
            var userReviews = await _platformContractReviewDbService.GetAll(userId);

            List<PlatformContractReviewMark> platformContractReviewMarks = new List<PlatformContractReviewMark>();
            foreach (var ur in userReviews)
            {
                platformContractReviewMarks.AddRange(ur.Marks);
            }

            var position = user.UserRoles.Any(ur => ur.Role.Name == RolesConstants.PlatformCustomer) ? RolesConstants.PlatformCustomer : (
                user.UserRoles.Any(ur => ur.Role.Name == RolesConstants.PlatformExecutorCompany) ? RolesConstants.PlatformExecutorCompany : (
                    user.UserRoles.Any(ur => ur.Role.Name == RolesConstants.PlatformExecutorMaster) ? RolesConstants.PlatformExecutorMaster : (
                        user.UserRoles.Any(ur => ur.Role.Name == RolesConstants.PlatformExecutorTeam) ? RolesConstants.PlatformExecutorTeam : ""
                    )
                )
            );
                        
            var userCategories = await _platformUserCategoryDbService.List(userId);
            var categories = await _platformCategoryDbService.List(userCategories.Select(uc => uc.PlatformCategory.ParentCategoryId ?? 0).Distinct().ToList());

            var company = await _platformCompanyDbService.Get(userId);

            return new PlatformPublicProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Avatar = user.Avatar,
                GoodReviews = reviews.Item1,
                BadReviews = reviews.Item2,
                Rating = await _platformUserRatingDbService.CalculateRating(userId),
                Resume = user.Resume,
                Position = position,
                Nationality = user.Nationality?.Name,
                AverageReview = userReviews.Count > 0 ? userReviews.Average(ur => ur.Marks.Average(m => m.Value)) : 0,
                Reviews = platformContractReviewMarks.GroupBy(p => p.DescriptionId).ToList().Select(x => new PlatformPublicProfileReviewDto {
                    TextId = x.Key,
                    Value = x.Average(y => y.Value)
                }).ToList(),
                Categories = categories.Select(c => c.NameId).ToList(),
                SubCategories = userCategories.Where(c => c.PlatformCategory.ParentCategoryId != null).Select(c => c.PlatformCategory.NameId).ToList(),
                PortfolioAlbums = (await _platformPortfolioAlbumDbService.List(userId, 0, 20)).Select(a => new PlatformPublicProfileAlbumDto {
                    Id = a.Id,
                    Description = a.Description,
                    Name = a.Name,
                    PublishDate = a.PublishDate,
                    Photos = a.PortfolioPhotos.Select(pp => $"/img/src/{pp.Value}").ToList()
                }).ToList(),
                PortfolioVideos = (await _platformPortfolioVideoDbService.List(userId)).Select(v => new PlatformPublicProfileVideoDto { 
                    Id = v.Id,
                    PublishDate = v.PublishDate,
                    Value = v.Value
                }).ToList(),
                Company = new PlatformPublicProfileCompanyDto
                {
                    Name = company?.Name,
                    About = company?.About
                },
                ContractsCompleted = await _platformContractDbService.CountFinished(userId),
                ContractsInWork = await _platformContractDbService.CountCurrent(userId),
                LoadStatus = "to do don't forget"
            };
        }
    }
}
