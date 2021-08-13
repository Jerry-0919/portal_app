using diga.web.Models.Images;
using diga.web.Models.Status;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("temp")]
        public async Task<ResponseData<ImageDto>> UploadTemp(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            MemoryStream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            await System.IO.File.WriteAllBytesAsync(
                Path.Combine(_environment.WebRootPath, "img", "temp", fileName), stream.ToArray());

            return new ResponseData<ImageDto>
            {
                Data = new ImageDto { FileName = fileName }
            };
        }

        [HttpPost("temp/list")]
        public async Task<ResponseData<List<ImageDto>>> UploadTemp(List<IFormFile> files)
        {
            List<ImageDto> data = new List<ImageDto> { };

            foreach (IFormFile file in files)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                MemoryStream stream = new MemoryStream();
                await file.CopyToAsync(stream);

                await System.IO.File.WriteAllBytesAsync(
                    Path.Combine(_environment.WebRootPath, "img", "temp", fileName), stream.ToArray());

                data.Add(new ImageDto { FileName = fileName });
            }

            return new ResponseData<List<ImageDto>>
            {
                Data = data
            };
        }
    }
}
