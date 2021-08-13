using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Reviews
    {
        [Key]
        public int Id { get; set; }
        public string NameTextId { get; set; }
        public string PictureUrl { get; set; }
        public string ReviewTextId { get; set; }
        public string PositionTextId { get; set; }
        public string CompanyName { get; set; }

        public virtual Texts NameText { get; set; }
        public virtual Texts PositionText { get; set; }
        public virtual Texts ReviewText { get; set; }
    }
}
