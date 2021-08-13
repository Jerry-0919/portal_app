using AutoMapper;
using diga.bl.Models;
using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.PlatformCategoryDbServices;
using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformCategories;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformCategoryServices
{
    public class PlatformCategoryService : IPlatformCategoryService
    {
        private readonly IPlatformCategoryDbService _platformCategoryDbService;

        public PlatformCategoryService(IPlatformCategoryDbService platformCategoryDbService)
        {
            _platformCategoryDbService = platformCategoryDbService;
        }

        public async Task<List<PlatformCategoryDto>> List()
        {
            var list = await _platformCategoryDbService.List();
            var result = new List<PlatformCategoryDto>();
            foreach (var l in list.Where(x => x.ParentCategoryId == null))
            {
                result.Add(new PlatformCategoryDto
                {
                    Id = l.Id,
                    NameId = l.NameId,
                    PlatformCategories = list.Where(ll => ll.ParentCategoryId == l.Id).Select(ll => new PlatformCategoryDto
                    {
                        Id = ll.Id,
                        NameId = ll.NameId
                    }).ToList()
                });
            }

            return result;
        }

        public async Task<List<PlatformCategoryDto>> ListPublished()
        {
            var list = await _platformCategoryDbService.ListPublished();
            var result = new List<PlatformCategoryDto>();
            foreach (var l in list.Where(x => x.ParentCategoryId == null))
            {
                result.Add(new PlatformCategoryDto
                {
                    Id = l.Id,
                    NameId = l.NameId,
                    PlatformCategories = list.Where(ll => ll.ParentCategoryId == l.Id).Select(ll => new PlatformCategoryDto
                    {
                        Id = ll.Id,
                        NameId = ll.NameId
                    }).ToList()
                });
            }

            return result;
        }
    }
}
