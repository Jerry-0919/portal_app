using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public partial class Questions
    {
        [Key]
        public int Id { get; set; }
        public string QuestionTextId { get; set; }
        public string AnswerTextId { get; set; }

        public string FullAnswerTextId { get; set; }
        public int? Type { get; set; }

        public virtual Texts AnswerText { get; set; }
        public virtual Texts QuestionText { get; set; }
        public virtual Texts FullAnswerText { get; set; }
    }
}
