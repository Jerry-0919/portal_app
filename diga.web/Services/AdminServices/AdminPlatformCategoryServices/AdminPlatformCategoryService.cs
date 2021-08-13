using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.PlatformCategoryDbServices;
using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformCategoryServices
{
    public class AdminPlatformCategoryService : IAdminPlatformCategoryService
    {
        private readonly IPlatformCategoryDbService _platformCategoryDbService;

        public AdminPlatformCategoryService(IPlatformCategoryDbService platformCategoryDbService)
        {
            _platformCategoryDbService = platformCategoryDbService;
        }

        public async Task<PaginatedList<AdminPlatformCategoryDto>> GetPaginatedList(int skip, int take)
        {
            var list = (await _platformCategoryDbService.ListParent(skip, take))
                    .Select(c => new AdminPlatformCategoryDto
                    {
                        Id = c.Id,
                        NameId = c.NameId,
                        ParentCategoryId = c.ParentCategoryId,
                        ReservationPercent = c.ReservationPercent
                    }).ToList();
            foreach (var l in list)
            {
                l.Children = (await _platformCategoryDbService.ListByParent(l.Id)).Select(c => new AdminPlatformCategoryDto
                {
                    Id = c.Id,
                    NameId = c.NameId,
                    ParentCategoryId = c.ParentCategoryId,
                    ReservationPercent = c.ReservationPercent
                }).ToList(); ;
            }
            return new PaginatedList<AdminPlatformCategoryDto>
            {
                CountAll = await _platformCategoryDbService.Count(),
                List = list
            };
        }

        public async Task<ResponseData> Add(AdminPlatformCategoryAddDto request)
        {
            await _platformCategoryDbService.Add(new PlatformCategory
            {
                NameId = request.NameId,
                ParentCategoryId = request.ParentCategoryId,
                ReservationPercent = request.ReservationPercent
            });

            return new ResponseData();
        }

        public async Task<ResponseData> Update(int id, AdminPlatformCategoryEditDto request)
        {
            PlatformCategory category = await _platformCategoryDbService.Get(id);

            category.NameId = request.NameId;
            category.ReservationPercent = request.ReservationPercent;
            category.ParentCategoryId = request.ParentCategoryId;

            await _platformCategoryDbService.Update(category);

            return new ResponseData();
        }

        public async Task<ResponseData> Remove(int id)
        {
            PlatformCategory category = await _platformCategoryDbService.Get(id);
            await _platformCategoryDbService.Remove(category);

            return new ResponseData();
        }

        public async Task<AdminPlatformCategoryDto> Get(int id)
        {
            var item = await _platformCategoryDbService.Get(id);
            return new AdminPlatformCategoryDto
            {
                Id = item.Id,
                NameId = item.NameId,
                ParentCategoryId = item.ParentCategoryId,
                ReservationPercent = item.ReservationPercent
            };
        }
    }
}
