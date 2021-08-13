using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Chats;
using diga.dal.DbServices.PlatformChatRoomDbServices;
using diga.dal.DbServices.PlatformChatRoomMessageDbServices;
using diga.dal.DbServices.PlatformChatRoomMessageReactionDbServices;
using diga.dal.DbServices.PlatformChatRoomUserDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.dal.Services;
using diga.web.Helpers;
using diga.web.Hubs;
using diga.web.Models.Hubs;
using diga.web.Models.PlatformChats;
using diga.web.Models.Status;
using diga.web.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformChatServices
{
    public class PlatformChatService : IPlatformChatService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IPlatformChatRoomDbService _platformChatRoomDbService;
        private readonly IPlatformChatRoomMessageDbService _platformChatRoomMessageDbService;
        private readonly IPlatformChatRoomMessageReactionDbService _platformChatRoomMessageReactionDbService;
        private readonly IPlatformChatRoomUserDbService _platformChatRoomUserDbService;
        private readonly IUserDbService _userDbService;
        private readonly ICultureService _cultureService;
        private readonly IHttpContextAccessor _httpContextAccessor;        
        private readonly HubUserCollection _hubUserCollection;
        private readonly IHubContext<ApplicationHub> _hubContext;
        private readonly EmailHelper _emailHelper;
        private readonly DomainOptions _domainOptions;

        public PlatformChatService(IWebHostEnvironment environment,
            IPlatformChatRoomDbService platformChatRoomDbService,
            IPlatformChatRoomMessageDbService platformChatRoomMessageDbService,
            IPlatformChatRoomMessageReactionDbService platformChatRoomMessageReactionDbService,
            IPlatformChatRoomUserDbService platformChatRoomUserDbService,
            IUserDbService userDbService,
            ICultureService cultureService,
            IHttpContextAccessor httpContextAccessor,
            HubUserCollection hubUserCollection,
            IHubContext<ApplicationHub> hubContext,
            EmailHelper emailHelper,
            IOptions<DomainOptions> domainOptions)
        {
            _environment = environment;
            _platformChatRoomDbService = platformChatRoomDbService;
            _platformChatRoomMessageDbService = platformChatRoomMessageDbService;
            _platformChatRoomMessageReactionDbService = platformChatRoomMessageReactionDbService;
            _userDbService = userDbService;
            _cultureService = cultureService;
            _httpContextAccessor = httpContextAccessor;
            _hubUserCollection = hubUserCollection;
            _platformChatRoomUserDbService = platformChatRoomUserDbService;
            _hubContext = hubContext;
            _emailHelper = emailHelper;
            _domainOptions = domainOptions.Value;
        }

        public async Task<List<PlatformChatRoomDto>> ListRooms(int userId, int skip, int take)
        {
            List<PlatformChatRoom> rooms = await _platformChatRoomDbService.List(userId, skip, take);
            foreach (var role in PlatformChatSystemRoles.Roles)
            {
                if (!rooms.Any(r => r.IsSystem == true && r.SystemRoleName == role))
                {
                    rooms.Insert(0, await _platformChatRoomDbService.Add(new PlatformChatRoom {
                        Avatar =  $"/Assets/platform/img/{role}.png",
                        IsSystem = true,
                        RoomName = role,
                        SystemRoleName = role,
                        Users = new List<PlatformChatRoomUser>()
                        {
                            new PlatformChatRoomUser{
                                ApplicationUserId = userId,
                                LastStatusChanged = DateTime.Now                                
                            }
                        }
                    }));
                }
            }

            List<PlatformChatRoomMessage> lastMessages =
                await _platformChatRoomMessageDbService.LastList(rooms.Select(r => r.Id).ToList());

            var list = rooms.Select(r =>
            {
                ApplicationUser user = r.Users.FirstOrDefault(u => u.ApplicationUserId != userId)?.ApplicationUser;
                PlatformChatRoomMessage lastMessage = lastMessages.FirstOrDefault(m => m.Sender.PlatformChatRoomId == r.Id);

                return new PlatformChatRoomDto
                {
                    RoomId = r.Id,
                    Avatar = string.IsNullOrEmpty(r.Avatar) ? $"/img/src/{user.Avatar}" : r.Avatar,
                    RoomName = r.RoomName,
                    UnreadCount = _platformChatRoomMessageDbService.UnreadCount(r.Id, userId),
                    IsSystem = r.IsSystem,
                    IsFavourite = r.Users.Any(u => u.IsFavourite == true && u.ApplicationUserId == userId),
                    LastMessage = lastMessage == null ? null : new PlatformChatMessageDto
                    {
                        Content = lastMessage?.Content,
                        Date = lastMessage?.DateTime.ToString("yyyy-MM-dd"),
                        Timestamp = lastMessage?.DateTime.ToString("HH:mm"),
                    },
                    Users = r.Users.Select(u => new PlatformChatUserDto
                    {
                        _id = u.ApplicationUserId,
                        Username = u.ApplicationUser.Name,
                        Avatar = string.IsNullOrEmpty(u.ApplicationUser.Avatar) ? null : $"/img/src/{u.ApplicationUser.Avatar}",
                        Status = new PlatformChatUserStatusDto
                        {
                            State = _hubUserCollection.Connections.ContainsKey(u.ApplicationUser.Id) ? "online" : "offline",
                            LastChanged = u.ApplicationUser.LastSeen == null ? "Never" : u.ApplicationUser.LastSeen.Value.ToString("yyyy-MM-dd")
                        }
                    }).ToList()
                };
            }).ToList();

            return list;
        }

        public async Task<List<PlatformChatMessageDto>> ListMessages(int userId, int roomId, int skip, int take)
        {
            PlatformChatRoom room = await _platformChatRoomDbService.GetFull(roomId);

            if (!room.Users.Any(u => u.ApplicationUserId == userId))
                return null;

            return (await _platformChatRoomMessageDbService.List(roomId, skip, take))
                .Select(m => new PlatformChatMessageDto
                {
                    _id = m.Id,
                    Content = m.Content,
                    SenderId = m.SenderApplicationUserId,
                    Username = m.Sender.ApplicationUser.Name,
                    Avatar = m.Sender.ApplicationUser.Avatar,
                    Date = m.DateTime.ToString("yyyy-MM-dd"),
                    Timestamp = m.DateTime.ToString("HH:mm"),
                    System = false,
                    New = false,
                    Distributed = m.Status == PlatformChatMessageStatuses.Saved,
                    Seen = m.Status == PlatformChatMessageStatuses.Seen,
                    DisableActions = true,
                    DisableReactions = false,
                    File = m.File == null ? null : new PlatformChatMessageFileDto
                    {
                        Name = m.File.Name,
                        Size = m.File.Size,
                        Type = Path.GetExtension(m.File.Name),
                        Audio = m.File.IsAudio,
                        Duration = m.File.Duration,
                        Url = $"{_httpContextAccessor.HttpContext.Request.PathBase}/docs/src/{m.File.FileName}"
                    },
                    Reactions = m.Reactions == null ? null : m.Reactions.Select(r => new PlatformChatMessageReactionDto
                    {
                        Value = r.Value
                    }).ToList()
                }).ToList();
        }

        public async Task<PlatformChatMessageDto> AddMessage(int userId, PlatformChatMessageAddDto request)
        {
            PlatformChatRoomMessageFile file = null;
            if (request.File != null)
            {
                string temp = Path.Combine(_environment.WebRootPath, "docs", "temp", request.File.FileName);
                string src = Path.Combine(_environment.WebRootPath, "docs", "src", $"{request.File.FileName}.{request.File.Extension}");

                File.Move(temp, src);
                FileInfo fileInfo = new FileInfo(src);

                bool isAudio = AudioHelper.IsAudio(src);

                file = new PlatformChatRoomMessageFile
                {
                    Name = $"{request.File.Name}.{request.File.Extension}",
                    FileName = $"{request.File.FileName}.{request.File.Extension}",
                    Size = fileInfo.Length,
                    IsAudio = isAudio,
                    Duration = isAudio ? AudioHelper.GetAudioDuration(src) : TimeSpan.Zero
                };
            }

            PlatformChatRoomMessage message = await _platformChatRoomMessageDbService.Add(new PlatformChatRoomMessage
            {
                Content = request.Content,
                DateTime = DateTime.UtcNow,
                File = file,
                Sender = (await _platformChatRoomUserDbService.Get(request.RoomId, userId)),
                Status = PlatformChatMessageStatuses.Saved,                
            });

            var result = new PlatformChatMessageDto
            {
                RoomId = message.Sender.PlatformChatRoomId,
                _id = message.Id,
                Content = message.Content,
                SenderId = userId,
                Username = message.Sender.ApplicationUser.Name,
                Avatar = message.Sender.ApplicationUser.Avatar,
                Date = message.DateTime.ToString("yyyy-MM-dd"),
                Timestamp = message.DateTime.ToString("HH:mm"),
                System = false,
                New = true,
                Distributed = true,
                Seen = false,
                DisableActions = true,
                DisableReactions = false,
                File = message.File == null ? null : new PlatformChatMessageFileDto
                {
                    Name = message.File.Name,
                    Size = message.File.Size,
                    Type = Path.GetExtension(message.File.Name),
                    Audio = message.File.IsAudio,
                    Duration = message.File.Duration,
                    Url = $"{_httpContextAccessor.HttpContext.Request.PathBase}/docs/src/{message.File.FileName}"
                },               
            };

            PlatformChatRoom room = await _platformChatRoomDbService.GetFull(request.RoomId);
            room.LastUpdated = DateTime.Now;
            await _platformChatRoomDbService.Update(room);
            var applicationUser = await _userDbService.GetFull(userId);

            //fixed chats - support / sales / suggestions
            if (room.IsSystem == true && !applicationUser.UserRoles.Any(ur => PlatformChatSystemRoles.Roles.Contains(ur.Role.Name)) && room.Users.Count == 1)
            {
                if (room.SystemRoleName == PlatformChatSystemRoles.Suggestion)
                {
                    await _emailHelper.SendAsync("geral@diga.pt", $"New suggestion from {message.Sender.ApplicationUser.Name}",
                        $"<b>Message</b>: {message.Content} <br> <b>Email</b>: {message.Sender.ApplicationUser.Email} <br> <b>Phone</b>: {message.Sender.ApplicationUser.PhoneNumber}");
                }
                else
                {
                    if (!room.Users.Any(u => u.ApplicationUser.UserRoles.Any(r => r.Role.Name == room.SystemRoleName)))
                    {
                        var roleUsers = await _userDbService.ListByRole(room.SystemRoleName);
                        var selectedAgent = roleUsers.OrderBy(r => Guid.NewGuid()).Where(roleUser => _hubUserCollection.Connections.ContainsKey(roleUser.Id)).FirstOrDefault();
                        if (selectedAgent != null)
                        {
                            room.Users.Add(new PlatformChatRoomUser
                            {
                                ApplicationUserId = selectedAgent.Id,
                                LastStatusChanged = DateTime.Now
                            });
                            await _platformChatRoomDbService.Update(room);
                            await _hubContext.Clients.Clients(_hubUserCollection.Connections[selectedAgent.Id].Select(c => c.ConnectionId).ToList()).SendAsync("ReceiveRoom", new PlatformChatRoomDto
                            {
                                Avatar = room.Avatar,
                                IsOnline = true,
                                RoomId = room.Id,
                                UnreadCount = 1,
                                IsSystem = true,
                                RoomName = room.RoomName,
                                LastMessage = new PlatformChatMessageDto
                                {
                                    Content = result.Content,
                                    Date = result.Date,
                                    Timestamp = result.Timestamp,
                                },
                                Users = room.Users.Select(u => new PlatformChatUserDto
                                {
                                    _id = u.ApplicationUserId,
                                    Username = u.ApplicationUser.Name,
                                    Avatar = string.IsNullOrEmpty(u.ApplicationUser.Avatar) ? null : $"/img/src/{u.ApplicationUser.Avatar}",
                                    Status = new PlatformChatUserStatusDto
                                    {
                                        State = _hubUserCollection.Connections.ContainsKey(u.ApplicationUser.Id) ? "online" : "offline",
                                        LastChanged = u.ApplicationUser.LastSeen == null ? "Never" : u.ApplicationUser.LastSeen.Value.ToString("yyyy-MM-dd")
                                    }
                                }).ToList()
                            });
                        }
                        else
                        {
                            foreach (var roleUser in roleUsers)
                            {
                                await _emailHelper.SendAsync(roleUser.Email, $"New request from {message.Sender.ApplicationUser.Name} ({room.SystemRoleName})",
                                    $"<b>Message</b>: {message.Content} <br> <a href=\"{_domainOptions.Client}/api/platform/chat/rooms/accept/{room.Id}/{roleUser.Id}\">Accept this request</a>");
                            }
                        }
                    }
                }
            }

            //sending notification to other users
            foreach (var user in room.Users.Where( u => userId != u.ApplicationUserId).ToList())
            {
                if (_hubUserCollection.Connections.ContainsKey(user.ApplicationUserId))
                {
                    await _hubContext.Clients.Clients(_hubUserCollection.Connections[user.ApplicationUserId].Select(c => c.ConnectionId).ToList()).SendAsync("ReceiveMessage", result);
                    message.Status = PlatformChatMessageStatuses.Distributed;
                    await _platformChatRoomMessageDbService.Update(message);
                }
                else
                {
                    if (user.ApplicationUser.NewMessagesEmailNotification == true) //only if mail notifications enabled
                    {
                        var countUnread = _platformChatRoomMessageDbService.UnreadCount(room.Id, user.ApplicationUserId);
                        var lastUnreadMessage = await _platformChatRoomMessageDbService.GetLastUnread(room.Id, user.ApplicationUserId);
                        var hours = lastUnreadMessage == null ? 0 : (DateTime.Now - lastUnreadMessage.DateTime).TotalHours;
                        if (lastUnreadMessage == null || hours > 1)
                        {
                            await _emailHelper.SendAsync(user.ApplicationUser.Email, $"{_cultureService.GetTranslation("you_have_new_message_from", applicationUser.Language ?? "pt")} {message.Sender.ApplicationUser.Name} - {_domainOptions.Client.Replace("https://", "").Replace("http://", "")}",
                                $"<b>{_cultureService.GetTranslation("message", applicationUser.Language ?? "pt")}</b> : {message.Content} <br> " +
                                $"<b>{_cultureService.GetTranslation("sender", applicationUser.Language ?? "pt")}</b> : {message.Sender.ApplicationUser.Name} <br> " +
                                $"<a href=\"{_domainOptions.Client}/platform/index#/chat\">{_cultureService.GetTranslation("read_and_answer", applicationUser.Language ?? "pt")}</a>");
                        }
                        else
                        {
                            if (countUnread == 5)
                            {
                                await _emailHelper.SendAsync(user.ApplicationUser.Email, $"{_cultureService.GetTranslation("you_have_new_messages_from", applicationUser.Language ?? "pt")} {message.Sender.ApplicationUser.Name} - {_domainOptions.Client.Replace("https://", "").Replace("http://", "")}",
                                    $"<b>{_cultureService.GetTranslation("unread_messages_count", applicationUser.Language ?? "pt")}</b> : {countUnread} <br> " +
                                    $"<b>{_cultureService.GetTranslation("sender", applicationUser.Language ?? "pt")}</b> : {message.Sender.ApplicationUser.Name} <br> " +
                                    $"<a href=\"{_domainOptions.Client}/platform/index#/chat\">{_cultureService.GetTranslation("read_and_answer", applicationUser.Language ?? "pt")}</a>");
                            }
                        }
                    }
                }
            }

            return result;
        }

        #region not used currently

        public async Task<PlatformChatMessageDto> EditMessage(int userId, PlatformChatMessageEditDto request)
        {
            PlatformChatRoomMessage message = await _platformChatRoomMessageDbService.GetFull(request.MessageId);
            //if (message.SenderId != userId)
            //    return null;

            message.Content = request.Content;
            await _platformChatRoomMessageDbService.Update(message);

            return new PlatformChatMessageDto
            {
                Content = message.Content,
                Date = message.DateTime.ToString("yyyy-MM-dd"),
                Timestamp = message.DateTime.ToString("HH:mm"),
                File = new PlatformChatMessageFileDto
                {
                    Audio = message.File.IsAudio,
                    Duration = message.File.Duration,
                    Name = message.File.Name,
                    Size = message.File.Size,
                    Type = message.File.FileName,
                    Url = $"{_httpContextAccessor.HttpContext.Request.PathBase}/src/{message.File.FileName}"
                }
            };
        }

        public async Task RemoveMessage(int userId, int messageId)
        {
            PlatformChatRoomMessage message = await _platformChatRoomMessageDbService.Get(messageId);
            //if (message.SenderId != userId)
            //    return;

            await _platformChatRoomMessageDbService.Remove(message);
        }

        public async Task AddRoom(int userId, List<int> userIds)
        {
            await _platformChatRoomDbService.Add(new PlatformChatRoom
            {
                Users = userIds.Select(u => new PlatformChatRoomUser
                {
                    ApplicationUserId = userId,
                    LastStatusChanged = DateTime.UtcNow
                }).ToList()
            });
        }

        public async Task AddMessageReaction(int userId, PlatformChatMessageSendReactionDto request)
        {
            if (!await _platformChatRoomMessageReactionDbService.Any(userId, request.MessageId, request.Value))
            {
                await _platformChatRoomMessageReactionDbService.Add(new PlatformChatRoomMessageReaction
                {
                    PlatformChatRoomMessageId = request.MessageId,
                    Value = request.Value,
                    UserId = userId
                });
            }
        }
        #endregion

        public async Task<ResponseData> ReadMessage(int userId, int messageId)
        {
            PlatformChatRoomMessage message = await _platformChatRoomMessageDbService.Get(messageId);
            message.Status = PlatformChatMessageStatuses.Seen;
            await _platformChatRoomMessageDbService.Update(message);
            return new ResponseData();
        }

        public async Task<ResponseData> AcceptRoom(int roomId, int userId)
        {
            var room = await _platformChatRoomDbService.GetFull(roomId);
            if (room.Users.Count == 2)
            {
                return ResponseData.Fail("error", "You cannot take this request. Other agent has been assigned.");
            }
            if (room.Users.Any(u => u.ApplicationUserId == userId))
            {
                return ResponseData.Fail("error", "You already assigned to this chat room");
            }
            room.Users.Add(new PlatformChatRoomUser {
                ApplicationUserId = userId,
                LastStatusChanged = DateTime.Now
            });
            await _platformChatRoomDbService.Update(room);
            return new ResponseData();
        }

        public async Task<ResponseData> AddToFavourites(int roomId, int userId)
        {
            var userRoom = await _platformChatRoomUserDbService.Get(roomId, userId);
            userRoom.IsFavourite = true;
            await _platformChatRoomUserDbService.Update(userRoom);
            return ResponseData.Ok();
        }

        public async Task<ResponseData> RemoveFromFavourites(int roomId, int userId)
        {
            var userRoom = await _platformChatRoomUserDbService.Get(roomId, userId);
            userRoom.IsFavourite = null;
            await _platformChatRoomUserDbService.Update(userRoom);
            return ResponseData.Ok();
        }

        public async Task<PlatformChatUserDto> AddSupportToRoom(int roomId)
        {
            PlatformChatRoom room = await _platformChatRoomDbService.GetFull(roomId);

            var roleUsers = (await _userDbService.ListByRole(PlatformChatSystemRoles.Support)).Where(roleUser => !room.Users.Select(u => u.ApplicationUserId).ToList().Contains(roleUser.Id)).ToList();

            var selectedAgent = roleUsers.OrderBy(r => Guid.NewGuid()).Where(roleUser => _hubUserCollection.Connections.ContainsKey(roleUser.Id)).FirstOrDefault();
            if (selectedAgent != null)
            {
                room.Users.Add(new PlatformChatRoomUser
                {
                    ApplicationUserId = selectedAgent.Id,
                    LastStatusChanged = DateTime.Now
                });
                await _platformChatRoomDbService.Update(room);
                await _hubContext.Clients.Clients(_hubUserCollection.Connections[selectedAgent.Id].Select(c => c.ConnectionId).ToList()).SendAsync("ReceiveRoom", new PlatformChatRoomDto
                {
                    Avatar = room.Avatar,
                    IsOnline = true,
                    RoomId = room.Id,
                    UnreadCount = 1,
                    IsSystem = true,
                    RoomName = room.RoomName,
                    LastMessage = new PlatformChatMessageDto
                    {
                        Content = "Technical Support Request",
                        Date = DateTime.Now.ToString("yyyy-MM-dd"),
                        Timestamp = DateTime.Now.ToString("HH:mm"),
                    },
                    Users = room.Users.Select(u => new PlatformChatUserDto
                    {
                        _id = u.ApplicationUserId,
                        Username = u.ApplicationUser.Name,
                        Avatar = string.IsNullOrEmpty(u.ApplicationUser.Avatar) ? null : $"/img/src/{u.ApplicationUser.Avatar}",
                        Status = new PlatformChatUserStatusDto
                        {
                            State = _hubUserCollection.Connections.ContainsKey(u.ApplicationUser.Id) ? "online" : "offline",
                            LastChanged = u.ApplicationUser.LastSeen == null ? "Never" : u.ApplicationUser.LastSeen.Value.ToString("yyyy-MM-dd")
                        }
                    }).ToList()
                });

                return new PlatformChatUserDto
                {
                    _id = selectedAgent.Id,
                    Username = selectedAgent.Name,
                    Avatar = string.IsNullOrEmpty(selectedAgent.Avatar) ? null : $"/img/src/{selectedAgent.Avatar}",
                    Status = new PlatformChatUserStatusDto
                    {
                        State = _hubUserCollection.Connections.ContainsKey(selectedAgent.Id) ? "online" : "offline",
                        LastChanged = selectedAgent.LastSeen == null ? "Never" : selectedAgent.LastSeen.Value.ToString("yyyy-MM-dd")
                    }
                };
            }
            else
            {
                foreach (var roleUser in roleUsers)
                {
                    await _emailHelper.SendAsync(roleUser.Email, $"New support request to room {room.RoomName}",
                        $"<a href=\"{_domainOptions.Client}/api/platform/chat/rooms/accept/{room.Id}/{roleUser.Id}\">Accept this request</a>");
                }
                return new PlatformChatUserDto();
            }
        }
    }
}
