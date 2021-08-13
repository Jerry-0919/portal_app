using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class UserTariffs
    {
        [Required, ForeignKey("Tariff")]
        public int TariffId { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool? IsTrial { get; set; }
        public double? CurrentPrice { get; set; }
        public double? PriceForNextUser { get; set; }
        public int? NumberOfUsers { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Tariffs Tariff { get; set; }
    }
}
