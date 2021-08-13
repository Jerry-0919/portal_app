namespace diga.web.Models.PlatformChats
{
    public class PlatformChatMessageAddDto
    {
        public int RoomId { get; set; }
        public string Content { get; set; }

        public PlatformChatMessageFileAddDto File { get; set; }
    }
}
