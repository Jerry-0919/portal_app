using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Modules
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BigTitleTextId { get; set; }
        public string SmallTitleTextId { get; set; }
        public string PictureUrl { get; set; }
        public string LongTextId { get; set; }
        public double Price { get; set; }
        public bool? Enabled { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
        public string LogoUrl { get; set; }
        public string CommentTextId { get; set; }
        public int? CartOrder { get; set; }

        public virtual Texts BigTitleText { get; set; }
        public virtual Texts LongText { get; set; }
        public virtual Texts SmallTitleText { get; set; }
        public virtual Texts CommentText { get; set; }
        public virtual ICollection<Functions> Functions { get; set; }
    }
}
