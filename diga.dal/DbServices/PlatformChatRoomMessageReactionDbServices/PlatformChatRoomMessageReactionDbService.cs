using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomMessageReactionDbServices
{
    public class PlatformChatRoomMessageReactionDbService : DefaultDbService<int, PlatformChatRoomMessageReaction>,
        IPlatformChatRoomMessageReactionDbService
    {
        private readonly DigaContext _db;

        public PlatformChatRoomMessageReactionDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int userId, int messageId, int value)
        {
            return await _db.PlatformChatRoomMessageReactions.AnyAsync(r => 
                r.UserId == userId && r.PlatformChatRoomMessageId == messageId && r.Value == value);
        }
    }
}
