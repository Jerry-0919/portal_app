using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomUserDbServices
{
    public class PlatformChatRoomUserDbService : ManyToManyDbService<PlatformChatRoomUser>,
        IPlatformChatRoomUserDbService
    {
        private readonly DigaContext _db;

        public PlatformChatRoomUserDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformChatRoomUser> Get(int roomId, int userId)
        {
            return await _db.PlatformChatRoomUsers.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.PlatformChatRoomId == roomId && p.ApplicationUserId == userId);
        }
    }
}
