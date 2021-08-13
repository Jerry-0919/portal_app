using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.ViewModels
{
    public class EupagoResponseModel
    {
        public string Reference { get; set; }
        public string Entity { get; set; }
        public string RedirectUrl { get; set; }
    }
}
