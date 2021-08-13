using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Chats
{
    public class PlatformChatRoomMessage : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformChatRoomUser")]
        public int SenderApplicationUserId { get; set; }

        [ForeignKey("PlatformChatRoomUser")]
        public int SenderPlatformChatRoomId { get; set; }

        public PlatformChatRoomUser Sender { get; set; }

        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        public string Status { get; set; }

        public PlatformChatRoomMessageFile File { get; set; }
        public List<PlatformChatRoomMessageReaction> Reactions { get; set; }
    }
}
