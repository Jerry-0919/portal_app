using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Chats
{
    public class PlatformChatRoomUser
    {
        [ForeignKey("PlatformChatRoom")]
        public int PlatformChatRoomId { get; set; }
        public PlatformChatRoom PlatformChatRoom { get; set; }
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime LastStatusChanged { get; set; }
        public bool? IsFavourite { get; set; }
    }
}
