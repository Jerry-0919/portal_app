using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace diga.web.Extensions
{
    public static class ValidationExtension
    {
        public static Dictionary<string, List<string>> ToErrors(this ValidationResult result)
        {
            return result.Errors
                    .Select(e => new KeyValuePair<string, string>(e.PropertyName, e.ErrorMessage))
                    .GroupBy(p => p.Key)
                    .ToDictionary(x => x.Key, x => x.Select(x => x.Value).ToList());
        }
    }
}
