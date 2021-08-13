using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomMessageReactionDbServices
{
    public interface IPlatformChatRoomMessageReactionDbService : IDefaultDbService<int, PlatformChatRoomMessageReaction>
    {
        Task<bool> Any(int userId, int messageId, int value);
    }
}
