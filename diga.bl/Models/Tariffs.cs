using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using diga.bl.Interfaces;

namespace diga.bl.Models
{
    public partial class Tariffs : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int NumberOfUsers { get; set; }
        public double? PriceForNextUser { get; set; }
        public int? Days { get; set; }

        public virtual ICollection<TariffModules> TariffModules { get; set; }
    }
}
