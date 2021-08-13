using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace diga.bl.Models.Platform.PaidFeatures
{
    public class PaidFeature : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Texts")]
        public int NameId { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; } // Валюта

    }
}
