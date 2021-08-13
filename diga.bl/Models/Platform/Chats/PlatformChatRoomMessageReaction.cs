using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Chats
{
    public class PlatformChatRoomMessageReaction : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformChatRoomMessage")]
        public int PlatformChatRoomMessageId { get; set; }
        public PlatformChatRoomMessage PlatformChatRoomMessage { get; set; }

        public int Value { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
