using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace diga.web.Helpers
{
    public class PlatformContractHelper
    {
        private readonly IWebHostEnvironment _environment;

        public PlatformContractHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void RemoveFiles(PlatformContract contract)
        {
            foreach (PlatformEstimate estimate in contract.PlatformEstimates)
            {
                string estimatePath = Path.Combine(_environment.WebRootPath, "docs", "src", estimate.FileName);

                if (File.Exists(estimatePath))
                    File.Delete(estimatePath);
            }

            foreach (PlatformContractFile file in contract.Files)
            {
                string filePath = Path.Combine(_environment.WebRootPath, "docs", "src", file.FileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
