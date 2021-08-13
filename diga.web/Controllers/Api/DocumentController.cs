using diga.web.Models.Documents;
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
    [Route("api/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public DocumentController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("temp")]
        public async Task<ResponseData<DocumentDto>> UploadTemp(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            MemoryStream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            await System.IO.File.WriteAllBytesAsync(
                Path.Combine(_environment.WebRootPath, "docs", "temp", fileName), stream.ToArray());

            return new ResponseData<DocumentDto>
            {
                Data = new DocumentDto { FileName = fileName }
            };
        }

        [HttpPost("temp/list")]
        public async Task<ResponseData<List<DocumentDto>>> UploadTemp(List<IFormFile> files)
        {
            List<DocumentDto> data = new List<DocumentDto> { };

            foreach (IFormFile file in files)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                MemoryStream stream = new MemoryStream();
                await file.CopyToAsync(stream);

                await System.IO.File.WriteAllBytesAsync(
                    Path.Combine(_environment.WebRootPath, "docs", "temp", fileName), stream.ToArray());

                data.Add(new DocumentDto { FileName = fileName });
            }

            return new ResponseData<List<DocumentDto>>
            {
                Data = data
            };
        }
    }
}
