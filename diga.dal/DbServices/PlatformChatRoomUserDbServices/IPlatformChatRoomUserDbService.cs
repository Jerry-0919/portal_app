using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomUserDbServices
{
    public interface IPlatformChatRoomUserDbService : IManyToManyDbService<PlatformChatRoomUser>
    {
        Task<PlatformChatRoomUser> Get(int roomId, int userId);
    }
}
