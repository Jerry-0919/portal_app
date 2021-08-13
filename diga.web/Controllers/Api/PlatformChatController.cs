using diga.web.Models.PlatformChats;
using diga.web.Models.Status;
using diga.web.Services.PlatformChatServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/chat")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformChatController : BaseController
    {
        private readonly IPlatformChatService _platformChatService;

        public PlatformChatController(IPlatformChatService platformChatService)
            : base(null, null)
        {
            _platformChatService = platformChatService;
        }

        [HttpGet("rooms/accept/{roomId}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptRoom(int roomId, int userId)
        {
            var result = await _platformChatService.AcceptRoom(roomId, userId);
            if (result.Errors.Count > 0)
            {
                return Ok(result.Errors);
            }
            else
            {
                return Redirect("/platform/index");
            }
        }

        [HttpGet("rooms/{skip}/{take}")]
        public async Task<List<PlatformChatRoomDto>> ListRooms(int skip, int take)
        {
            return await _platformChatService.ListRooms(UserId, skip, take);
        }

        [HttpGet("messages/{roomId}/{skip}/{take}")]
        public async Task<List<PlatformChatMessageDto>> ListMessages(int roomId, int skip, int take)
        {
            return await _platformChatService.ListMessages(UserId, roomId, skip, take);
        }

        [HttpPost("messages/send")]
        public async Task<PlatformChatMessageDto> SendMessage(PlatformChatMessageAddDto message)
        {
            return await _platformChatService.AddMessage(UserId, message);
        }

        [HttpPost("messages/read/{messageId}")]
        public async Task<ResponseData> SendMessage(int messageId)
        {
            return await _platformChatService.ReadMessage(UserId, messageId);
        }

        [HttpPost("rooms/favourites/{roomId}")]
        public async Task<ResponseData> AddRoomToFavourites(int roomId)
        {
            return await _platformChatService.AddToFavourites(roomId, UserId);
        }

        [HttpDelete("rooms/favourites/{roomId}")]
        public async Task<ResponseData> RemoveRoomFromFavourites(int roomId)
        {
            return await _platformChatService.RemoveFromFavourites(roomId, UserId);
        }

        [HttpPost("rooms/support/add/{roomId}")]
        public async Task<PlatformChatUserDto> AddSupportToRoom(int roomId)
        {
            return await _platformChatService.AddSupportToRoom(roomId);
        }
    }
}
