using System;

namespace diga.web.Models.PlatformContractDiscussions
{
    public class PlatformContractDiscussionDto
    {
        public int UserId { get; set; }
        public int DiscussionId { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}
