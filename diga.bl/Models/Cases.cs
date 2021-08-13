using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Cases
    {
        [Key]
        public int Id { get; set; }
        public string TitleTextId { get; set; }
        public string ShortTextId { get; set; }
        public string LongTextId { get; set; }
        public string ReviewFileUrl { get; set; }
        public string ReviewVideoUrl { get; set; }
        public string CompanyName { get; set; }

        public virtual Texts LongText { get; set; }
        public virtual Texts ShortText { get; set; }
        public virtual Texts TitleText { get; set; }
    }
}
