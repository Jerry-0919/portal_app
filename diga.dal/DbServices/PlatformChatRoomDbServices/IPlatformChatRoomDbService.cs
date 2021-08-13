using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomDbServices
{
    public interface IPlatformChatRoomDbService : IDefaultDbService<int, PlatformChatRoom>
    {
        Task<PlatformChatRoom> Get(int contractId, int userId, int secondUserId);
        Task<List<PlatformChatRoom>> List(int userId, int skip, int take);
        Task<List<int>> ListIds(int userId);
        Task<PlatformChatRoom> GetFull(int roomId);
    }
}
