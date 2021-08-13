using diga.bl.Constants;
using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomMessageDbServices
{
    public class PlatformChatRoomMessageDbService : DefaultDbService<int, PlatformChatRoomMessage>,
        IPlatformChatRoomMessageDbService
    {
        private readonly DigaContext _db;

        public PlatformChatRoomMessageDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformChatRoomMessage> GetFull(int messageId)
        {
            return await _db.PlatformChatRoomMessages.Include(m => m.Sender)
                .GroupBy(m => m.Sender.PlatformChatRoomId).Select(m => m.Last()).FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<PlatformChatRoomMessage> GetLastUnread(int roomId, int userId)
        {
            return await _db.PlatformChatRoomMessages.OrderByDescending(m => m.DateTime).FirstOrDefaultAsync(m => m.Status != PlatformChatMessageStatuses.Seen && m.SenderPlatformChatRoomId == roomId && m.SenderApplicationUserId != userId);
        }

        public async Task<List<PlatformChatRoomMessage>> LastList(List<int> roomIds)
        {
            return _db.PlatformChatRoomMessages.Include(m => m.Sender).AsEnumerable()
                .Where(m => roomIds.Contains(m.Sender.PlatformChatRoomId))
                .GroupBy(m => m.Sender.PlatformChatRoomId).Select(m => m.Last()).ToList();
        }

        public async Task<List<PlatformChatRoomMessage>> List(int roomId, int skip, int take)
        {
            return await _db.PlatformChatRoomMessages.Include(m => m.File).Include(m => m.Sender).ThenInclude(s => s.ApplicationUser)
                .Where(m => m.SenderPlatformChatRoomId == roomId).OrderByDescending(m => m.DateTime).Skip(skip).Take(take).ToListAsync();
        }

        public int UnreadCount(int roomId, int userId)
        {
            return _db.PlatformChatRoomMessages.Where(m => m.Status != PlatformChatMessageStatuses.Seen && m.SenderPlatformChatRoomId == roomId && m.SenderApplicationUserId != userId).OrderByDescending(m => m.DateTime).Count();
        }

        public int UnreadCount(List<int> roomIds, int userId)
        {
            return _db.PlatformChatRoomMessages.Where(m => m.Status != PlatformChatMessageStatuses.Seen && m.SenderApplicationUserId != userId && roomIds.Contains(m.SenderPlatformChatRoomId)).OrderByDescending(m => m.DateTime).Count();
        }
    }
}
