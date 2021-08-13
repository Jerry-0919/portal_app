using diga.bl.Models;
using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.PlatformPortfolioVideoDbServices;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformPortfolioVideos;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPortfolioVideoServices
{
    public class PlatformPortfolioVideoService : IPlatformPortfolioVideoService
    {
        private readonly IPlatformPortfolioVideoDbService _platformPortfolioVideoDbService;

        public PlatformPortfolioVideoService(IPlatformPortfolioVideoDbService platformPortfolioVideoDbService)
        {
            _platformPortfolioVideoDbService = platformPortfolioVideoDbService;
        }

        public async Task<PaginatedList<PlatformPortfolioVideoDto>> List(int userId, int skip, int take)
        {
            return new PaginatedList<PlatformPortfolioVideoDto>
            {
                List = (await _platformPortfolioVideoDbService.List(userId, skip, take))
               .Select(a => new PlatformPortfolioVideoDto
               {
                   Id = a.Id,
                   Value = a.Value,
                   PublishDate = a.PublishDate
               }).ToList(),
                CountAll = await _platformPortfolioVideoDbService.Count(userId)
            };
        }

        public async Task<List<PlatformPortfolioVideoDto>> List(int userId)
        {
            return (await _platformPortfolioVideoDbService.List(userId))
               .Select(a => new PlatformPortfolioVideoDto
               {
                   Id = a.Id,
                   Value = a.Value,
                   PublishDate = a.PublishDate
               }).ToList();
        }

        public async Task<ResponseData> Add(int userId, PlatformPortfolioVideoAddDto request)
        {
            await _platformPortfolioVideoDbService.Add(new PlatformPortfolioVideo
            {
                ApplicationUserId = userId,
                Value = request.Value,
                PublishDate = DateTime.UtcNow
            });

            return new ResponseData();
        }

        public async Task<ResponseData> Edit(int userId, int videoId, PlatformPortfolioVideoEditDto request)
        {
            PlatformPortfolioVideo video = await _platformPortfolioVideoDbService.Get(videoId);

            if (video.ApplicationUserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            video.Value = request.Value;

            return new ResponseData();
        }

        public async Task<ResponseData> Remove(int userId, int videoId)
        {
            PlatformPortfolioVideo video = await _platformPortfolioVideoDbService.Get(videoId);

            if (video.ApplicationUserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            await _platformPortfolioVideoDbService.Remove(video);
            return new ResponseData();
        }
    }
}
