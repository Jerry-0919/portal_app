using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace diga.bl.Models
{
    public class Articles
    {
        [Key]
        public int Id { get; set; }

        public string TextId { get; set; }
        public string TitleTextId { get; set; }
        public bool? Enabled { get; set; }

        public virtual Texts Text { get; set; }
        public virtual Texts TitleText { get; set; }

    }
}
