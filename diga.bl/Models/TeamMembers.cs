using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class TeamMembers
    {
        [Key]
        public int Id { get; set; }
        public string NameTextId { get; set; }
        public string PhotoUrl { get; set; }
        public string DepartmentTextId { get; set; }
        public string PositionTextId { get; set; }
        public string CountryTextId { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileIcon { get; set; }

        public virtual Texts CountryText { get; set; }
        public virtual Texts DepartmentText { get; set; }
        public virtual Texts NameText { get; set; }
        public virtual Texts PositionText { get; set; }
    }
}
