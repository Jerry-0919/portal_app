using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomDbServices
{
    public class PlatformChatRoomDbService : DefaultDbService<int, PlatformChatRoom>, IPlatformChatRoomDbService
    {
        private readonly DigaContext _db;

        public PlatformChatRoomDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformChatRoom> Get(int contractId, int userId, int secondUserId)
        {
            return await _db.PlatformChatRooms.Include(r => r.Users).ThenInclude(u => u.ApplicationUser).FirstOrDefaultAsync(r => r.PlatformContractId == contractId 
            && r.Users.Any(u => u.ApplicationUserId == userId) 
            && r.Users.Any(u => u.ApplicationUserId == secondUserId));
        }

        public async Task<PlatformChatRoom> GetFull(int roomId)
        {
            return await _db.PlatformChatRooms.Include(r => r.Users).ThenInclude(u => u.ApplicationUser).ThenInclude(a => a.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<List<PlatformChatRoom>> List(int userId, int skip, int take)
        {
            return await _db.PlatformChatRooms.Include(r => r.Users).ThenInclude(u => u.ApplicationUser)
                .Where(r => r.Users.Any(u => u.ApplicationUserId == userId)).OrderByDescending(r => r.IsSystem).ThenByDescending(r => r.LastUpdated).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<int>> ListIds(int userId)
        {
            return await _db.PlatformChatRooms.Where(r => r.Users.Any(u => u.ApplicationUserId == userId)).Select(r => r.Id).ToListAsync();
        }
    }
}
