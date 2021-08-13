using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace diga.bl.Models
{
    public class Carts
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public int NumberOfUsers { get; set; }
        public int Term { get; set; }
        [ForeignKey("Tariff")]
        public int TariffId { get; set; }
        public string Modules { get; set; }
        public double TotalPrice { get; set; }
        public double? TotalPriceWithDiscount { get; set; }
        public double? FromBalance { get; set; }
        public string Reference { get; set; }
        public string Provider { get; set; }
        public bool? IsPaid { get; set; }
        public string Country { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Tariffs Tariff { get; set; }
    }
}
