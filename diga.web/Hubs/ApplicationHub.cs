using diga.web.Models.Hubs;
using diga.web.Models.PlatformChats;
using diga.web.Models.PlatformNotifications;
using diga.web.Services.PlatformChatServices;
using diga.web.Services.PlatformNotificationServices;
using diga.web.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace diga.web.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationHub : Hub
    {
        private readonly HubUserCollection _hubUserCollection;
        private readonly IPlatformNotificationService _platformNotificationService;
        private readonly IPlatformChatService _platformChatService;
        private readonly IUserService _userService;

        public ApplicationHub(HubUserCollection hubUserCollection,
            IPlatformNotificationService platformNotificationService,
            IPlatformChatService platformChatService,
            IUserService userService)
        {
            _hubUserCollection = hubUserCollection;
            _platformNotificationService = platformNotificationService;
            _platformChatService = platformChatService;
            _userService = userService;
        }

        private int UserId { get { return int.Parse(((ClaimsIdentity)Context.User.Identity).FindFirst("Id").Value); } }

        #region Connections
        public override async Task OnConnectedAsync()
        {
            HubUser notificationUser = new HubUser { ConnectionId = Context.ConnectionId };

            if (_hubUserCollection.Connections.ContainsKey(UserId))
                _hubUserCollection.Connections[UserId].Add(notificationUser);
            else
                _hubUserCollection.Connections.TryAdd(UserId, new List<HubUser> { notificationUser });

            await _userService.LastSeen(UserId, DateTime.Now);
            await Clients.All.SendAsync("ReceiveUserIsOnline", new { userId = UserId, lastChanged = DateTime.Now.ToString("yyyy-MM-dd") });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ClaimsIdentity identity = (ClaimsIdentity)Context.User.Identity;
            int userId = int.Parse(identity.FindFirst("Id").Value);

            if (_hubUserCollection.Connections[userId].Count != 1)
            {
                _hubUserCollection.Connections[userId].Remove(
                    _hubUserCollection.Connections[userId].First(c => c.ConnectionId == Context.ConnectionId));
            }
            else
            {
                _hubUserCollection.Connections.TryRemove(userId, out _);
            }
            await Clients.All.SendAsync("ReceiveUserIsOffline", new { userId = UserId, lastChanged = DateTime.Now.ToString("yyyy-MM-dd") });

            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region Notifications

        [HubMethodName("read-notification")]
        public async Task ReadNotification(int notificationId)
        {
            await _platformNotificationService.Read(notificationId);
        }

        #endregion
    }
}
