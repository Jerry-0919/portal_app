using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.ViewModels
{
    public class CartModel
    {
        public int NumberOfUsers { get; set; }
        public int TariffId { get; set; }
        public string ModuleIds { get; set; }
        public string AuthToken { get; set; }
        public string Promocode { get; set; }
        public int Term { get; set; }
        public string Country { get; set; }
    }
}
