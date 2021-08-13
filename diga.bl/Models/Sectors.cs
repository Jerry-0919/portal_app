using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public partial class Sectors
    {
        [Key]
        public int Id { get; set; }
        public string BigTitleTextId { get; set; }
        public string SmallTitleTextId { get; set; }
        public string PictureUrl { get; set; }
        public string LongTextId { get; set; }
        public bool? Enabled { get; set; }
        [ForeignKey("FullDescription")]
        public string FullDescriptionTextId { get; set; }

        public virtual Texts FullDescription { get; set; }
    }
}
