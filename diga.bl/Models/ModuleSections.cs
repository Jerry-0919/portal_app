using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace diga.bl.Models
{
    public class ModuleSections
    {
        [Key]
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ButtonTextId { get; set; }
        public string TextId { get; set; }

        public virtual Modules Module { get; set; }
        public virtual Texts ButtonText { get; set; }
        public virtual Texts Text { get; set; }
    }
}
