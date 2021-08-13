using diga.bl.Models; using diga.dal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.Services
{
    public interface ICultureService
    {
        string GetTranslation(string textId, string language);
    }

    public class CultureService: ICultureService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public CultureService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public string GetTranslation(string textId, string language) {
            var translation = "";

            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DigaContext>();

                switch (language)
                {
                    case "en":
                        translation = db.Texts.FirstOrDefault(a => a.TextId == textId)?.En;
                        break;
                    case "ru":
                        translation = db.Texts.FirstOrDefault(a => a.TextId == textId)?.Ru;
                        break;
                    case "pt":
                        translation = db.Texts.FirstOrDefault(a => a.TextId == textId)?.Pt;
                        break;
                    case "es":
                        translation = db.Texts.FirstOrDefault(a => a.TextId == textId)?.Es;
                        break;
                    default:
                        translation = db.Texts.FirstOrDefault(a => a.TextId == textId)?.En;
                        break;
                }
            }


            return translation ?? "";
        }
    }
}
