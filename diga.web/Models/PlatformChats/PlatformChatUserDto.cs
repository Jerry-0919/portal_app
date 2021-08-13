using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformChats
{
    public class PlatformChatUserDto
    {
        public int _id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }

        public PlatformChatUserStatusDto Status { get; set; }
    }
}
