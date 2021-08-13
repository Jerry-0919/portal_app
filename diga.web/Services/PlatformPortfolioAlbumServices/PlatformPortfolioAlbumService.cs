using diga.bl.Models;
using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.PlatformAlbumCategoryDbServices;
using diga.dal.DbServices.PlatformPortfolioAlbumDbServices;
using diga.dal.DbServices.PlatformPortfolioPhotoDbServices;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformPortfolioAlbums;
using diga.web.Models.Status;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformPortfolioAlbumServices
{
    public class PlatformPortfolioAlbumService : IPlatformPortfolioAlbumService
    {
        private readonly IPlatformPortfolioAlbumDbService _platformPortfolioAlbumDbService;
        private readonly IPlatformPortfolioPhotoDbService _platformPortfolioPhotoDbService;
        private readonly IPlatformAlbumCategoryDbService _platformAlbumCategoryDbService;
        private readonly IWebHostEnvironment _environment;

        public PlatformPortfolioAlbumService(IPlatformPortfolioAlbumDbService platformPortfolioAlbumDbService,
            IPlatformPortfolioPhotoDbService platformPortfolioPhotoDbService,
            IPlatformAlbumCategoryDbService platformAlbumCategoryDbService,
            IWebHostEnvironment environment)
        {
            _platformPortfolioAlbumDbService = platformPortfolioAlbumDbService;
            _platformPortfolioPhotoDbService = platformPortfolioPhotoDbService;
            _platformAlbumCategoryDbService = platformAlbumCategoryDbService;
            _environment = environment;
        }

        public async Task<PaginatedList<PlatformPortfolioAlbumDto>> List(int userId, int skip, int take)
        {
            return new PaginatedList<PlatformPortfolioAlbumDto>
            {
                List = (await _platformPortfolioAlbumDbService.List(userId, skip, take))
               .Select(a => new PlatformPortfolioAlbumDto
               {
                   Id = a.Id,
                   Name = a.Name,
                   Description = a.Description,
                   PublishDate = a.PublishDate,
                   Categories = a.AlbumCategories.Select(pp => new PlatformPortfolioAlbumCategoryDto
                   {
                       Id = pp.PlatformCategoryId,
                       NameId = pp.PlatformCategory?.NameId,
                       ParentCategoryId = pp.PlatformCategory?.ParentCategoryId
                   }).ToList(),
                   Photoes = a.PortfolioPhotos.Select(pp => pp.Value).ToList()
               }).ToList(),
                CountAll = await _platformPortfolioAlbumDbService.Count(userId)
            };
        }

        public async Task<ResponseData> Add(int userId, PlatformPortfolioAlbumAddDto request)
        {
            // Validations

            if (request.Description.Length > 400)
                return ResponseData.Fail("Description", "The description cannot be longer than 400 characters!");

            if (request.Photos.Count > 16)
                return ResponseData.Fail("Photos", "You can select maximum 16 photos!");

            // End validations

            PlatformPortfolioAlbum album = await _platformPortfolioAlbumDbService.Add(new PlatformPortfolioAlbum
            {
                ApplicationUserId = userId,
                Name = request.Name,
                Description = request.Description,
                PublishDate = DateTime.UtcNow
            });

            for (int i = 0; i < request.Photos.Count; i++)
            {
                string tempPath = Path.Combine(_environment.WebRootPath, "img", "temp", request.Photos[i]);
                string srcPath = Path.Combine(_environment.WebRootPath, "img", "src", request.Photos[i]);

                if (File.Exists(tempPath))
                {
                    File.Move(tempPath, srcPath);
                }
                else
                {
                    request.Photos.RemoveAt(i);
                    i--;
                }
            }

            await _platformPortfolioPhotoDbService.AddRange(request.Photos.Select(p => new PlatformPortfolioPhoto
            {
                PortfolioAlbumId = album.Id,
                Value = p
            }));

            await _platformAlbumCategoryDbService.AddRange(request.CategoryIds.Select(c => new PlatformAlbumCategory
            {
                PortfolioAlbumId = album.Id,
                PlatformCategoryId = c
            }));

            return new ResponseData();
        }

        public async Task<ResponseData> Edit(int userId, int albumId, PlatformPortfolioAlbumEditDto request)
        {
            // Validations

            if (request.Description.Length > 400)
                return ResponseData.Fail("Description", "The description cannot be longer than 400 characters!");

            if (request.Photos.Count > 16)
                return ResponseData.Fail("Photos", "You can select maximum 16 photos!");

            PlatformPortfolioAlbum album = await _platformPortfolioAlbumDbService.Get(albumId);

            if (album.ApplicationUserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            // End validations

            album.Name = request.Name;
            album.Description = request.Description;

            List<PlatformAlbumCategory> categories = await _platformAlbumCategoryDbService.List(album.Id);
            await _platformAlbumCategoryDbService.RemoveRange(categories.Where(c => !request.CategoryIds.Contains(c.PlatformCategoryId)));
            await _platformAlbumCategoryDbService.AddRange(request.CategoryIds.Where(c => !categories.Any(ac => ac.PlatformCategoryId == c))
                .Select(c => new PlatformAlbumCategory { PlatformCategoryId = c, PortfolioAlbumId = album.Id }));

            List<PlatformPortfolioPhoto> photos = await _platformPortfolioPhotoDbService.List(album.Id);
            List<PlatformPortfolioPhoto> photosToRemove = photos.Where(p => !request.Photos.Contains(p.Value)).ToList();
            List<PlatformPortfolioPhoto> photosToAdd = request.Photos.Where(p => !photos.Any(ap => ap.Value == p))
                .Select(p => new PlatformPortfolioPhoto { PortfolioAlbumId = album.Id, Value = p }).ToList();

            foreach (PlatformPortfolioPhoto photo in photos)
            {
                string path = Path.Combine(_environment.WebRootPath, "img", "src", photo.Value);
                if (File.Exists(path))
                    File.Delete(path);
            }

            for (int i = 0; i < photosToAdd.Count; i++)
            {
                string tempPath = Path.Combine(_environment.WebRootPath, "img", "temp", photosToAdd[i].Value);
                string srcPath = Path.Combine(_environment.WebRootPath, "img", "src", photosToAdd[i].Value);

                if (File.Exists(tempPath))
                {
                    File.Move(tempPath, srcPath);
                }
                else
                {
                    request.Photos.RemoveAt(i);
                    i--;
                }
            }

            await _platformPortfolioPhotoDbService.RemoveRange(photosToRemove);
            await _platformPortfolioPhotoDbService.AddRange(photosToAdd);

            return new ResponseData();
        }

        public async Task<ResponseData> Remove(int userId, int albumId)
        {
            PlatformPortfolioAlbum album = await _platformPortfolioAlbumDbService.Get(albumId);

            if (album.ApplicationUserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            List<PlatformPortfolioPhoto> photos = await _platformPortfolioPhotoDbService.List(album.Id);

            foreach (PlatformPortfolioPhoto photo in photos)
            {
                string path = Path.Combine(_environment.WebRootPath, "img", "src", photo.Value);
                if (File.Exists(path))
                    File.Delete(path);
            }

            await _platformPortfolioAlbumDbService.Remove(album);
            return new ResponseData();
        }
    }
}
