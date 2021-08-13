using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.Services;
using diga.web.Services.UserServices;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.DocumentServices.EstimateService
{
    public class DocumentEstimateService: IDocumentEstimateService
    {
        private readonly IConverter _converter;
        private readonly IUserService _userService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly ICultureService _cultureService;
        public DocumentEstimateService(IConverter converter, IUserService userService, IPlatformEstimateDbService platformEstimateDbService, ICultureService cultureService)
        {
            _converter = converter;
            _userService = userService;
            _platformEstimateDbService = platformEstimateDbService;
            _cultureService = cultureService;
        }
        public async Task<byte[]> Pdf(int contractId, int userId, string language)
        {
            var estimate = await _platformEstimateDbService.Get(contractId, userId);
            if (estimate == null)
            {
                throw new Exception($"User {userId} does not have any estimate for contract {contractId}");
            }

            var estimateWithSections = await _platformEstimateDbService.GetWithSections(estimate.Id);
            var user = await _userService.Get(userId);

            // Smirno Code 23/06/2021
            var document_no = $"{contractId}-{estimateWithSections.Id}";

            // Smirno Code 23/06/2021




            var html = $@"
   <!DOCTYPE html>
   <html lang=""en"">
   <head>
       This is the header of this document.
   </head>
  <body>
  <h1>{_cultureService.GetTranslation("estimate", language)} : {contractId}-{estimateWithSections.Id}</h1>
  <p>This is a line of text for demonstration purposes only.</p>
  </body>
  </html>
  ";
            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = ColorMode.Color;
            globalSettings.Orientation = Orientation.Portrait;
            globalSettings.PaperSize = PaperKind.A4;
            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
            ObjectSettings objectSettings = new ObjectSettings();
            objectSettings.PagesCount = true;
            objectSettings.HtmlContent = html;
            WebSettings webSettings = new WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            HeaderSettings headerSettings = new HeaderSettings();
            headerSettings.FontSize = 10;
            headerSettings.FontName = "Ariel";
            headerSettings.Right = "[page] / [toPage]";
            headerSettings.Line = false;
            FooterSettings footerSettings = new FooterSettings();
            footerSettings.FontSize = 12;
            footerSettings.FontName = "Ariel";
            footerSettings.Center = $"pec.pt {DateTime.Now.Year}";
            footerSettings.Line = false;
            objectSettings.HeaderSettings = headerSettings;
            objectSettings.FooterSettings = footerSettings;
            objectSettings.WebSettings = webSettings;
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            return _converter.Convert(htmlToPdfDocument);
        }
    }
}
