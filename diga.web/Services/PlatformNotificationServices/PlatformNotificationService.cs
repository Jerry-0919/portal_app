using diga.bl.Models.Platform.Notifications;
using diga.dal.DbServices.PlatformNotificationDbServices;
using diga.web.Hubs;
using diga.web.Models.Hubs;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformNotifications;
using diga.web.Models.Status;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformNotificationServices
{
    public class PlatformNotificationService : IPlatformNotificationService
    {
        private readonly IPlatformNotificationDbService _platformNotificationDbService;
        private readonly IHubContext<ApplicationHub> _hubContext;
        private readonly HubUserCollection _hubUserCollection;

        public PlatformNotificationService(IPlatformNotificationDbService platformNotificationDbService,
             IHubContext<ApplicationHub> hubContext, HubUserCollection hubUserCollection)
        {
            _platformNotificationDbService = platformNotificationDbService;
            _hubUserCollection = hubUserCollection;
            _hubContext = hubContext;
        }

        public async Task<PlatformNotificationListDto> List(int userId, int skip, int take)
        {
            return new PlatformNotificationListDto
            {
                List = (await _platformNotificationDbService.List(userId, skip, take)).Select(n =>
                new PlatformNotificationDto
                {
                    Id = n.Id,
                    IsRead = n.IsRead,
                    DateTime = n.DateTime,
                    Type = n.Type,
                    Datas = n.Datas.Select(d => new PlatformNotificationDataDto
                    {
                        Key = d.Key,
                        Value = d.Value
                    }).ToList()
                })
                .ToList(),
                CountAll = await _platformNotificationDbService.Count(userId),
                CountUnread = await _platformNotificationDbService.CountUnread(userId)
            };
        }

        public async Task AddNotification(int userId, PlatformNotificationDto notification)
        {
            await _platformNotificationDbService.Add(new PlatformNotification
            {
                ApplicationUserId = userId,
                DateTime = notification.DateTime,
                Type = notification.Type,
                IsRead = notification.IsRead,
                Datas = notification.Datas.Select(d => new PlatformNotificationData
                {
                    Key = d.Key,
                    Value = d.Value
                }).ToList()
            });

            if (_hubUserCollection.Connections.ContainsKey(userId))
            {
                //todo
                try
                {
                    await _hubContext.Clients.Clients(_hubUserCollection.Connections[userId].Select(c => c.ConnectionId).ToList())
                        .SendAsync("ReceiveNotification", notification);
                }
                catch (Exception ex)
                {
                }

            }
        }

        public async Task<ResponseData> Read(int notificationId)
        {
            PlatformNotification notification = await _platformNotificationDbService.Get(notificationId);
            notification.IsRead = true;
            await _platformNotificationDbService.Update(notification);
            return new ResponseData();
        }
    }
}
