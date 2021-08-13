using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Helpers
{
    public class LanguageStorage: ILanguageStorage
    {
        public string Language { get; set; }

        public string GetLanguage() 
        {
            return Language;
        }
        public void SetLanguage(string language)
        {
            Language = language;
        }
    }
}
