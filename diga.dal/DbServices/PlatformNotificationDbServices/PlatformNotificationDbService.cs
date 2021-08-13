using diga.bl.Models.Platform.Notifications;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformNotificationDbServices
{
    public class PlatformNotificationDbService : DefaultDbService<int, PlatformNotification>, IPlatformNotificationDbService
    {
        private readonly DigaContext _db;

        public PlatformNotificationDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<int> Count(int userId)
        {
            return await _db.PlatformNotifications.CountAsync(n => n.ApplicationUserId == userId);
        }

        public async Task<int> CountUnread(int userId)
        {
            return await _db.PlatformNotifications.CountAsync(n => n.ApplicationUserId == userId && !n.IsRead);
        }

        public async Task<List<PlatformNotification>> List(int userId, int skip, int take)
        {
            return await _db.PlatformNotifications.Include(n => n.Datas).Where(n => n.ApplicationUserId == userId)
                .OrderByDescending(n => n.DateTime).Skip(skip).Take(take).ToListAsync();
        }
    }
}
