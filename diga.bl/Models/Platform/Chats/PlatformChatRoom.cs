using diga.bl.Interfaces;
using diga.bl.Models.Platform.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Chats
{
    public class PlatformChatRoom : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string Avatar { get; set; }
        public bool? IsSystem { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string SystemRoleName { get; set; }

        [ForeignKey("PlatformContract")]
        public int? PlatformContractId { get; set; }
        public List<PlatformChatRoomUser> Users { get; set; }
        public PlatformContract PlatformContract { get; set; }

    }
}
