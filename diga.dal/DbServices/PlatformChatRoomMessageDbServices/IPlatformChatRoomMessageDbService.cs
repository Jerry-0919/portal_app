using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformChatRoomMessageDbServices
{
    public interface IPlatformChatRoomMessageDbService : IDefaultDbService<int, PlatformChatRoomMessage>
    {
        Task<List<PlatformChatRoomMessage>> LastList(List<int> lists);
        Task<List<PlatformChatRoomMessage>> List(int roomId, int skip, int take);
        int UnreadCount(int roomId, int userId);
        int UnreadCount(List<int> roomIds, int userId);
        Task<PlatformChatRoomMessage> GetFull(int messageId);
        Task<PlatformChatRoomMessage> GetLastUnread(int roomId, int userId);
    }
}
