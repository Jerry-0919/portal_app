using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class Functions
    {
        [Key]
        public int Id { get; set; }
        public string BigTitleTextId { get; set; }
        public string SmallTitleTextId { get; set; }
        public string PictureUrl { get; set; }
        public string LongTextId { get; set; }
        [ForeignKey("FullDescription")]
        public string FullDescriptionTextId { get; set; }
        public int? ModuleId { get; set; }
        public bool? Enabled { get; set; }
        public string Url { get; set; }
        public virtual Modules Module { get; set; }
        public virtual Texts BigTitleText { get; set; }
        public virtual Texts SmallTitleText { get; set; }
        public virtual Texts LongText { get; set; }
        public virtual Texts FullDescription { get; set; }
    }
}
