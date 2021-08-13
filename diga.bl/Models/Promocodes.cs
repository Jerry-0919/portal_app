using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Promocodes
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public double? ToBalance { get; set; }
        public int? ToPeople { get; set; }
        public double? Discount { get; set; }
    }
}
