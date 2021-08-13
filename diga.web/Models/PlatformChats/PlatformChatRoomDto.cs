using System.Collections.Generic;

namespace diga.web.Models.PlatformChats
{
    public class PlatformChatRoomDto
    {
        public int RoomId { get; set; }
        public string Avatar { get; set; }
        public string RoomName { get; set; }
        public bool IsOnline { get; set; }
        public int UnreadCount { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsFavourite { get; set; }
        public PlatformChatMessageDto LastMessage { get; set; }
        public List<PlatformChatUserDto> Users { get; set; }
    }
}
