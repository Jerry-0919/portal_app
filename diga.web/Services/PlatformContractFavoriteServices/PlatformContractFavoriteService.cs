using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformContractFavoriteDbServices;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractFavorites;
using diga.web.Models.Status;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractFavoriteServices
{
    public class PlatformContractFavoriteService : IPlatformContractFavoriteService
    {
        private readonly IPlatformContractFavoriteDbService _platformContractFavoriteDbService;

        public PlatformContractFavoriteService(IPlatformContractFavoriteDbService platformContractFavoriteDbService)
        {
            _platformContractFavoriteDbService = platformContractFavoriteDbService;
        }

        public async Task<PaginatedList<PlatformContractFavoriteDto>> GetPaginatedList(int userId, int skip, int take)
        {
            return new PaginatedList<PlatformContractFavoriteDto>
            {
                List = (await _platformContractFavoriteDbService.List(userId, skip, take))
                    .Select(f => new PlatformContractFavoriteDto
                    {
                        Id = f.PlatformContract.Id,
                        Type = f.PlatformContract.ContractType?.Name,
                        Name = f.PlatformContract.Name,
                        Categories = f.PlatformContract.Categories.Select(c => c.PlatformCategory.NameId).ToList(),
                        PublishDate = f.PlatformContract.PublishDate,
                        PlannedBuildEnd = f.PlatformContract.PlannedBuildEnd,
                        EditStatus = f.PlatformContract.EditStatus,
                        Version = f.PlatformContract.VersionNumber,
                        VersionStatus = f.PlatformContract.VersionStatus,
                        Status = f.PlatformContract.Status
                    }).ToList(),
                CountAll = await _platformContractFavoriteDbService.Count(userId)
            };
        }

        public async Task<ResponseData> Add(int userId, int contractId)
        {
            if (!await _platformContractFavoriteDbService.Any(userId, contractId))
            {
                await _platformContractFavoriteDbService.Add(new PlatformContractFavorite
                {
                    ApplicationUserId = userId,
                    PlatformContractId = contractId
                });
            }

            return new ResponseData();
        }

        public async Task<ResponseData> Remove(int userId, int contractId)
        {
            if (await _platformContractFavoriteDbService.Any(userId, contractId))
            {
                PlatformContractFavorite favorite = await _platformContractFavoriteDbService.Get(userId, contractId);
                await _platformContractFavoriteDbService.Remove(favorite);
            }

            return new ResponseData();
        }
    }
}
