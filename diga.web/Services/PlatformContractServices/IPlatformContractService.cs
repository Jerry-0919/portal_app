using diga.bl.Enums.PlatformContracts;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformContracts;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractServices
{
    public interface IPlatformContractService
    {
        Task<PlatformContractAddResponseDto> AddPlatformContract(int userId);
        Task<ResponseData> UpdatePlatformContractInfo(int userId, int contractId, PlatformContractInfoDto request);
        Task<ResponseData> UpdatePlatformContractEstimate(int userId, int contractId, PlatformContractEstimateDto request);
        Task<ResponseData> UpdatePlatformContractPrice(int userId, int contractId, PlatformContractPriceDto request);
        Task<ResponseData> UpdatePlatformContractDates(int userId, int contractId, PlatformContractDatesDto request);
        Task<ResponseData> CompletePlatformContract(int userId, int contractId);
        Task<PaginatedList<PlatformContractDto>> GetPaginatedList(int userId, int skip, int take, string filter, List<string> statuses, SortType sort, bool isActual);
        Task<PlatformContractFullDto> Get(int userId, int id);
        Task<PlatformContractContractorDto> GetContractorDto(int userId, int id);
        Task<PlatformContractProgressDto> GetExecutorDto(int userId, int contractId);
        Task<ResponseData> Remove(int userId, int contractId);
        Task<ResponseData<PlatformContractCloneResponseDto>> Clone(int userId, PlatformContractCloneDto request);
        Task<PaginatedList<PlatformContractDto>> GetPaginatedListVersions(int userId, int id, int skip, int take);
        Task<ResponseData> SetTenderEnd(int userId, int contractId, PlatformContractTenderEndRequestDto request);
        Task<ResponseData> Archive(int userId, int contractId);
        Task<ResponseData> Refuse(int userId, int contractId, PlatformContractRefuseRequestDto request);
        Task<ResponseData> Close(int userId, int contractId, PlatformContractCloseRequestDto request);
        Task<ResponseData> Finish(int userId, int contractId);

        Task<PaginatedList<PlatformContractPublishedDto>> GetPublished(bool hideMyBids, int? balanceId, string filter, SortType sort, List<int> categories, List<string> tags, int userId, int skip, int take);
        
        /// <summary>
        /// Execution progress
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Task<PlatformContractExecutionDto> GetExecution(int userId, int contractId);
    }
}
