using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Chats
{
    public class PlatformChatRoomMessageFile
    {
        [Key, ForeignKey("PlatformChatRoomMessage")]
        public int Id { get; set; }

        public PlatformChatRoomMessage PlatformChatRoomMessage { get; set; }

        public string Name { get; set; }
        public string FileName { get; set; }

        public long Size { get; set; }
        public bool IsAudio { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
