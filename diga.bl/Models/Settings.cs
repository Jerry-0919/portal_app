using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class Settings
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
       
    }
}
