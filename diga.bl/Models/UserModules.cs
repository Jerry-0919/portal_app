using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class UserModules
    {
        [Required, ForeignKey("Module")]
        public int ModuleId { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool? IsTrial { get; set; }
        public double? CurrentPrice { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Modules Module { get; set; }
    }
}
