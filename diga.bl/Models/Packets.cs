using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Packets
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public string Tariff {get;set;}        
        public double? TariffPrice {get;set;}        
        public int? NumberOfUsers { get; set; }
        public double? PriceForNextUser { get; set; }
        public int? TrialDays { get; set; }
        public int? Days { get; set; }


        public virtual ICollection<PacketModules> PacketModules { get; set; }
    }
}
