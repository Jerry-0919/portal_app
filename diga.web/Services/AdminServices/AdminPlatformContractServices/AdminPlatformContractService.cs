using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.web.Helpers;
using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using diga.web.Services.PlatformEstimateServices;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformContractServices
{
    public class AdminPlatformContractService : IAdminPlatformContractService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformEstimateService _platformEstimateService;
        private readonly PlatformContractHelper _platformContractHelper;

        public AdminPlatformContractService(IPlatformContractDbService platformContractDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformEstimateService platformEstimateService,
            PlatformContractHelper platformContractHelper)
        {
            _platformEstimateDbService = platformEstimateDbService;
            _platformContractDbService = platformContractDbService;
            _platformEstimateService = platformEstimateService;
            _platformContractHelper = platformContractHelper;
        }
        public async Task<AdminPlatformContractFullDto> Get(int id)
        {
            PlatformContract contract = await _platformContractDbService.GetFull(id);
            PlatformEstimate estimate = contract.PlatformEstimates.FirstOrDefault(e => e.CreatorId == contract.UserId);

            return new AdminPlatformContractFullDto
            {
                Id = contract.Id,
                Name = contract.Name,
                Address = contract.Address,
                AddressHidden = contract.AddressHidden,
                Description = contract.Description,
                Price = contract.Price,
                Status = contract.Status,
                BuildStart = contract.BuildStart,
                PublishDate = contract.PublishDate,
                PlannedBuildEnd = contract.PlannedBuildEnd,
                TenderEndDate = contract.TenderEndDate,
                Categories = contract.Categories.Select(c => c.PlatformCategory.NameId).ToList(),
                Tags = contract.Tags.Select(t => t.PlatformTag.Name).ToList(),
                Files = contract.Files.Select(c => c.FileName).ToList(),
                Balance = contract.Balance == null ? null :
                    new AdminPlatformBalanceDto
                    {
                        Id = contract.Balance.Id,
                        Value = contract.Balance.Value
                    },
                City = contract.City == null ? null :
                    new AdminPlatformCityDto
                    {
                        Id = contract.City.Id,
                        Name = contract.City.Name
                    },
                ContractPriority = contract.ContractPriority == null ? null :
                    new AdminPlatformContractPriorityDto
                    {
                        Id = contract.ContractPriority.Id,
                        Value = contract.ContractPriority.Value
                    },
                ContractType = contract.ContractType == null ? null :
                    new AdminPlatformContractTypeDto
                    {
                        Id = contract.ContractType.Id,
                        Name = contract.ContractType.Name
                    },
                Estimate = estimate == null ? null :
                    new AdminPlatformEstimateDto
                    {
                        Id = estimate.Id,
                        Name = estimate.Name,
                        FileName = estimate.FileName,
                        IsOrdered = estimate.IsOrdered
                    },
                User = new AdminUserDto
                {
                    Id = contract.User.Id,
                    Balance = contract.User.Balance,
                    Domain = contract.User.Domain,
                    Email = contract.User.Email,
                    Language = contract.User.Language,
                    Name = contract.User.Name,
                    PhoneNumber = contract.User.PhoneNumber
                },
                CreatedDate = contract.CreatedDate,
                UpdatedDate = contract.UpdatedDate
            };
        }

        public async Task<PaginatedList<AdminPlatformContractDto>> GetPaginatedList(int skip, int take)
        {
            return new PaginatedList<AdminPlatformContractDto>
            {
                CountAll = await _platformContractDbService.Count(),
                List = (await _platformContractDbService.List(skip, take))
                    .Select(c => new AdminPlatformContractDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Address = c.Address,
                        Description = c.Description,
                        Price = c.Price,
                        BuildStart = c.BuildStart,
                        PlannedBuildEnd = c.PlannedBuildEnd
                    }).ToList()
            };
        }

        public async Task<ResponseData> Add(AdminPlatformContractAddDto request)
        {
            PlatformContract contract = await _platformContractDbService.Add(new PlatformContract
            {
                Name = request.Name,
                Address = request.Address,
                AddressHidden = request.AddressHidden,
                BalanceId = request.BalanceId,
                BuildStart = request.BuildStart,
                CityId = request.CityId,
                ContractPriorityId = request.ContractPriorityId,
                ContractTypeId = request.ContractTypeId,
                Description = request.Description,
                PlannedBuildEnd = request.PlannedBuildEnd,
                Price = request.Price,
                PublishDate = request.PublishDate,
                Status = request.Status,
                TenderEndDate = request.TenderEndDate,
                UserId = request.UserId
            });

            await _platformEstimateService.ParseAdd(request.Estimate.Name, request.Estimate.FileName, request.Estimate.IsOrdered, contract.Id, contract.UserId, 1);
            return new ResponseData();
        }

        public async Task<ResponseData> Update(int id, AdminPlatformContractEditDto request)
        {
            PlatformContract contract = await _platformContractDbService.Get(id);

            contract.Address = request.Address;
            contract.AddressHidden = request.AddressHidden;
            contract.BalanceId = request.BalanceId;
            contract.BuildStart = request.BuildStart;
            contract.CityId = request.CityId;
            contract.ContractPriorityId = request.ContractPriorityId;
            contract.ContractTypeId = request.ContractTypeId;
            contract.Description = request.Description;
            contract.Name = request.Name;
            contract.PlannedBuildEnd = request.PlannedBuildEnd;
            contract.Price = request.Price;
            contract.PublishDate = request.PublishDate;
            contract.Status = request.Status;
            contract.TenderEndDate = request.TenderEndDate;
            contract.UserId = request.UserId;

            await _platformContractDbService.Update(contract);

            PlatformEstimate estimate = await _platformEstimateDbService.Get(contract.Id, contract.UserId);

            if (estimate != null)
            {
                if (estimate.FileName == request.Estimate.FileName)
                {
                    estimate.Name = request.Estimate.Name;
                    estimate.IsOrdered = request.Estimate.IsOrdered;
                    await _platformEstimateDbService.Update(estimate);

                    return new ResponseData();
                }

                await _platformEstimateService.Remove(estimate);
            }

            await _platformEstimateService.ParseAdd(request.Estimate.Name, request.Estimate.FileName, request.Estimate.IsOrdered, contract.Id, contract.UserId, 1);

            return new ResponseData();
        }

        public async Task<ResponseData> Remove(int id)
        {
            PlatformContract contract = await _platformContractDbService.Get(id);

            _platformContractHelper.RemoveFiles(contract);
            await _platformContractDbService.Remove(contract);

            return new ResponseData();
        }
    }
}
