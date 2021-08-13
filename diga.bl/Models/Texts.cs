using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Texts
    {
        public Texts()
        {
        }

        [Key]
        public string TextId { get; set; }
        public string En { get; set; }
        public string Ru { get; set; }
        public string Pt { get; set; }
        public string Es { get; set; }
        public bool? IsHtml { get; set; }

    }
}
