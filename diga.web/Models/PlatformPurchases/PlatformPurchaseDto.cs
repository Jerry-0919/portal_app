using diga.bl.Enums.PlatformPurchases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformPurchases
{
    public class PlatformPurchaseDto
    {
        
        public int UserId { get; set; }

        public DateTime PaidDate { get; set; } 

        public int PaidFeatureId { get; set; }

        public double Price { get; set; }

        public bool IsPaid { get; set; }

        public double DebitedFromBalance { get; set; }

        public double DebitedFromCard { get; set; } 

        public PaymentProvider PaymentProvider { get; set; }

        public string Reference { get; set; }

        public DateTime ActualPaidDate { get; set; }
    }
}
