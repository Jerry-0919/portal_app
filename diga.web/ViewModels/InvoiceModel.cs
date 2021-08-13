using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.ViewModels
{
    public class InvoiceModel
    {
        public int CartId { get; set; }
        public string Language { get; set; }
        public string Companyname { get; set; }        
        public string Tin { get; set; }
        public string Legaladdress { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double PriceForUsers { get; set; }
        public double PriceForModules { get; set; }
        public double PromocodeSum { get; set; }
        public double TermSum { get; set; }
        public double Balance { get; set; }


    }
}
