using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Helpers
{
    public interface ILanguageStorage
    {
        string GetLanguage();
        void SetLanguage(string language);
    }
}
