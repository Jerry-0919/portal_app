using System;

namespace diga.web.Models.PlatformChats
{
    public class PlatformChatMessageFileDto
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public bool Audio { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
