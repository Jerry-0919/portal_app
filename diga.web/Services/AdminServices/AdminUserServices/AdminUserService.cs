using diga.bl.Constants;
using diga.bl.Models;
using diga.dal.DbServices.UserDbServices;
using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminUserServices
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserDbService _userDbService;

        public AdminUserService(IUserDbService userDbService)
        {
            _userDbService = userDbService;
        }

        public async Task<PaginatedList<AdminUserDto>> GetPaginatedList(int skip, int take,
            AdminPlatformUserFilterDto filter)
        {
            return new PaginatedList<AdminUserDto>
            {
                CountAll = await _userDbService.Count(filter.VerificationRequested),
                List = (await _userDbService.ListWithVerifications(skip, take, filter.VerificationRequested))
                .Select(a => new AdminUserDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Balance = a.Balance,
                    Domain = a.Domain,
                    Email = a.Email,
                    Language = a.Language,
                    PhoneNumber = a.PhoneNumber,
                    VerificationRequested = a.PlatformUserVerifications.OrderBy(v => v.Created).FirstOrDefault()?
                        .Status == PlatformUserVerificationStatuses.UnderConsideration
                }).ToList()
            };
        }

        public async Task<ResponseData> Update(int id, AdminUserEditDto request)
        {
            ApplicationUser user = await _userDbService.Get(id);

            user.Name = request.Name;
            user.Language = request.Language;
            user.PhoneNumber = request.PhoneNumber;
            user.Balance = request.Balance;
            user.Domain = request.Domain;

            await _userDbService.Update(user);
            return new ResponseData();
        }

        public async Task<AdminUserFullDto> Get(int id)
        {
            ApplicationUser user = await _userDbService.GetWithVerifications(id);

            return new AdminUserFullDto
            {
                Id = id,
                Email = user.Email,
                Balance = user.Balance,
                Domain = user.Domain,
                Language = user.Language,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Verifications = user.PlatformUserVerifications.Select(v => new AdminUserVerificationDto
                {
                    Status = v.Status,
                    Created = v.Created,
                    Photos = v.Photos.Select(p => p.Value).ToList()
                }).ToList()
            };
        }
    }
}
