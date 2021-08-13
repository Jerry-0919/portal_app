using diga.bl.Constants;
using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformUserVerificationDbServices;
using diga.web.Models.PlatformVerification;
using diga.web.Models.Status;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformVerificationServices
{
    public class PlatformVerificationService : IPlatformVerificationService
    {
        private readonly IPlatformUserVerificationDbService _platformUserVerificationDbService;
        private readonly IWebHostEnvironment _environment;

        public PlatformVerificationService(IPlatformUserVerificationDbService platformUserVerificationDbService,
            IWebHostEnvironment environment)
        {
            _platformUserVerificationDbService = platformUserVerificationDbService;
            _environment = environment;
        }

        public async Task<ResponseData> Apply(PlatformVerificationApplyDto request, int userId)
        {
            if (request.Photos.Count == 0)
                return ResponseData.Fail("Photos", "No photos!");

            if (request.Photos.Count > 3)
                return ResponseData.Fail("Photos", "Too many photos!");

            PlatformUserVerification last = await _platformUserVerificationDbService.GetLast(userId);
            if (last == null || last.Status == PlatformUserVerificationStatuses.Rejected)
            {
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

                await _platformUserVerificationDbService.Add(new PlatformUserVerification
                {
                    ApplicationUserId = userId,
                    Created = DateTime.UtcNow,
                    Status = PlatformUserVerificationStatuses.UnderConsideration,
                    Photos = request.Photos.Select(v => new PlatformUserVerificationPhoto
                    {
                        Value = v
                    }).ToList()
                });

                return ResponseData.Ok();
            }

            return ResponseData.Fail("Status", "You have already applied!");
        }
    }
}
