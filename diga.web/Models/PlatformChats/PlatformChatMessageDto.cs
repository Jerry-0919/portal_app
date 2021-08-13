using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformChats
{
    public class PlatformChatMessageDto
    {
        public int RoomId { get; set; }
        public int _id { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Date { get; set; }
        public string Timestamp { get; set; }
        public bool System { get; set; }
        public bool New { get; set; }
        public bool Distributed { get; set; }
        public bool Seen { get; set; }
        public bool DisableActions { get; set; }
        public bool DisableReactions { get; set; }

        public PlatformChatMessageFileDto File { get; set; }
        public List<PlatformChatMessageReactionDto> Reactions { get; set; }
    }
}
