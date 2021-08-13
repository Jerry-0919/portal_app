using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.ViewModels
{
    public class CreditCardModel
    {
        public string Name { get; set; }
        public int CartId { get; set; }
        public string CardNumber { get; set; }
        public string ValidToMm { get; set; }
        public string ValidToYy { get; set; }
        public string Cvv { get; set; }
    }
}
