using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class PacketModules
    {
        [Required, ForeignKey("Packet")]
        public int PacketId { get; set; }

        [Required, ForeignKey("Module")]
        public int ModuleId { get; set; }
        public double? Price { get; set; }

        public virtual Modules Module { get; set; }
        public virtual Packets Packet { get; set; }
    }
}
