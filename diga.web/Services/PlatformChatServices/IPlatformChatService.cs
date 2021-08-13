using diga.web.Models.PlatformChats;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformChatServices
{
    public interface IPlatformChatService
    {
        Task<List<PlatformChatRoomDto>> ListRooms(int userId, int skip, int take);
        Task<List<PlatformChatMessageDto>> ListMessages(int userId, int roomId, int skip, int take);
        Task<PlatformChatMessageDto> AddMessage(int userId, PlatformChatMessageAddDto request);
        Task<PlatformChatMessageDto> EditMessage(int userId, PlatformChatMessageEditDto request);
        Task RemoveMessage(int userId, int messageId);
        Task AddRoom(int userId, List<int> userIds);
        Task AddMessageReaction(int userId, PlatformChatMessageSendReactionDto request);
        Task<ResponseData> ReadMessage(int userId, int messageId);
        Task<ResponseData> AcceptRoom(int roomId, int userId);
        Task<ResponseData> AddToFavourites(int roomId, int userId);
        Task<ResponseData> RemoveFromFavourites(int roomId, int userId);
        Task<PlatformChatUserDto> AddSupportToRoom(int roomId);
    }
}
