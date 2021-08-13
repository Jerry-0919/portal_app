using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class TariffModules
    {
        [Required, ForeignKey("Tariff")]
        public int TariffId { get; set; }

        [Required, ForeignKey("Module")]
        public int ModuleId { get; set; }
        public int? Order { get; set; }

        public virtual Modules Module { get; set; }
        public virtual Tariffs Tariff { get; set; }
    }
}
