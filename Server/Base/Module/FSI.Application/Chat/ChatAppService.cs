using FSI.Application.Contracts.Chat.DTO;
using FSI.Application.Contracts.Chat.IService;
using FSI.Application.Contracts.CommonDto;
using FSI.Application.Hubs;
using FSI.Domain.Chat;
using FSI.Domain.File;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace FSI.Application.Chat
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ChatAppService : ApplicationService, IChatAppService
    {
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IUserConversationRepository _userConversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly Guid currentUserId;
        private readonly IConfiguration Configuration;
        private readonly IBlobContainer _blobContainer;

        public ChatAppService(IConversationRepository conversationRepository, IUserConversationRepository userConversationRepository, IMessageRepository messageRepository, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository, IFileInfomationRepository fileInfomationRepository, IConfiguration configuration, IBlobContainer blobContainer = null)
        {
            _conversationRepository = conversationRepository;
            _userConversationRepository = userConversationRepository;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _userRepository = userRepository;
            _fileInfomationRepository = fileInfomationRepository;
            Configuration = configuration;
            _blobContainer = blobContainer;
        }

        public async Task<PagedResultDto<ConversationDto>> PostToGetListConversation(GetListConversationDto input)
        {
            var users = await _userRepository.GetListAsync();
            var conversations = await _conversationRepository.GetQueryableAsync();
            var userConversations = await _userConversationRepository.GetQueryableAsync();
            var messages = await _messageRepository.GetListAsync();

            List<Conversation> conversation1s = new List<Conversation>();
            List<Conversation> conversation2s = new List<Conversation>();

            // chia theo type
            switch (input.Type)
            {
                case 0:
                    // lấy các conversation hai người theo filter và type 0
                    conversation1s = conversations.Where(c => c.JustTwoPeople.Value &&
                   ((c.UserAId.Equals(currentUserId) && c.UserB.Name.Contains(input.Filter) && c.IsActiveA.Value && !c.IsStorageA.Value) ||
                   ((c.UserBId.Equals(currentUserId) && c.UserA.Name.Contains(input.Filter) && c.IsActiveB.Value && !c.IsStorageB.Value)))).ToList();

                    // lấy các conversation còn lại theo filter và type 0
                    conversation2s = (from c in conversations
                                      join uc in userConversations
                                      on c.Id equals uc.ConversationId
                                      where uc.UserId.Equals(currentUserId) &&
                                      c.ConversationName.Contains(input.Filter) && uc.IsActive.Value && !uc.IsStorage.Value
                                      select c).ToList();
                    break;
                case 1:
                    // lấy các conversation hai người theo filter và type 1
                    conversation1s = conversations.Where(c => c.JustTwoPeople.Value &&
                   ((c.UserAId.Equals(currentUserId) && c.UserB.Name.Contains(input.Filter) && c.IsActiveA.Value && c.IsStorageA.Value) ||
                   ((c.UserBId.Equals(currentUserId) && c.UserA.Name.Contains(input.Filter) && c.IsActiveB.Value && c.IsStorageB.Value)))).ToList();

                    // lấy các conversation còn lại theo filter và type 1
                    conversation2s = (from c in conversations
                                      join uc in userConversations
                                      on c.Id equals uc.ConversationId
                                      where uc.UserId.Equals(currentUserId) &&
                                      c.ConversationName.Contains(input.Filter) && uc.IsActive.Value && uc.IsStorage.Value
                                      select c).ToList();
                    break;
                case 2:
                    // lấy các conversation hai người theo filter và type 2
                    conversation1s = conversations.Where(c => c.JustTwoPeople.Value &&
                   ((c.UserAId.Equals(currentUserId) && c.UserB.Name.Contains(input.Filter) && !c.IsActiveA.Value) ||
                   ((c.UserBId.Equals(currentUserId) && c.UserA.Name.Contains(input.Filter) && !c.IsActiveB.Value)))).ToList();

                    // lấy các conversation còn lại theo filter và type 2
                    conversation2s = (from c in conversations
                                      join uc in userConversations
                                      on c.Id equals uc.ConversationId
                                      where uc.UserId.Equals(currentUserId) &&
                                      c.ConversationName.Contains(input.Filter) && !uc.IsActive.Value
                                      select c).ToList();
                    break;
            }

            var myconversations = conversation1s.Concat(conversation2s).ToList();

            // lấy lastMessage và order theo tin nhắn cuối
            myconversations = myconversations.Join(messages, x => x.LastMessageId, y => y.Id, (x, y) =>
            {
                x.LastMessage = y;
                return x;
            }).OrderByDescending(x => x.LastMessage.CreationTime).ToList();



            var pagedConversations = myconversations.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();



            var rs = ObjectMapper.Map<List<Conversation>, List<ConversationDto>>(pagedConversations);
            var userCons = userConversations.ToList();
            // setConversationName và avatarUrl cho justTwoPeople
            // set isSeen
            rs.ForEach(c =>
            {
                if (c.JustTwoPeople.Value)
                {
                    if (c.UserAId.Equals(currentUserId))
                    {
                        c.ConversationName = c.UserB.Name;
                        c.ConversationAvatar = c.UserB.AvatarUrl;
                        if (c.LastIndexSeenA == c.LastMessage.Index)
                        {
                            c.IsSeen = true;
                        }
                        else c.IsSeen = false;
                    }
                    else
                    {
                        c.ConversationName = c.UserA.Name;
                        c.ConversationAvatar = c.UserA.AvatarUrl;
                        if (c.LastIndexSeenB == c.LastMessage.Index)
                        {
                            c.IsSeen = true;
                        }
                        else c.IsSeen = false;
                    }
                }
                else
                {
                    var userConversation = userCons.FirstOrDefault(x => x.ConversationId.Equals(c.Id) && x.UserId.Equals(currentUserId));
                    if (userConversation?.LastIndexSeen == c.LastMessage.Index)
                    {
                        c.IsSeen = true;
                    }
                    else c.IsSeen = false;
                }
            });

            return new PagedResultDto<ConversationDto>()
            {
                Items = rs,
                TotalCount = myconversations.Count
            };

        }

        public async Task<PagedResultDto<MessageDto>> PostToGetListMessageByConversation(GetListMessageDto input)
        {
            var conversation = await _conversationRepository.GetAsync(input.ConversationId);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(input.ConversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            var users = await _userRepository.GetListAsync();
            var messages = await _messageRepository.GetListAsync(x => x.ConversationId.Equals(input.ConversationId));

            var rs = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            
            rs.ForEach(x =>
            {
                x.IsMine = x.Sender.Id.Equals(currentUserId);
                if(x.FocusToMessageId != null)
                {
                    var focusMessage = messages.FirstOrDefault(y=> y.Id == x.FocusToMessageId);
                    x.FocusToMessage = ObjectMapper.Map<Message, MessageDto>(focusMessage);
                }
            });
            return new PagedResultDto<MessageDto>()
            {
                Items = rs,
                TotalCount = messages.Count
            };
        }

        /// <summary>
        /// Chỉ dùng để nhắn tin cho người lạ ( chưa có conversation)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<SendMessageToNewOtherResultDto> SendMessageToNewOther(MessageSendToUserDto input)
        {
            var oldConversation = await _conversationRepository.FindAsync(x => x.JustTwoPeople.Value &&
            ((x.UserAId.Equals(currentUserId) && x.UserBId.Equals(input.UserId)) || (x.UserBId.Equals(currentUserId) && x.UserAId.Equals(input.UserId))));

            if (oldConversation != null)
                throw new UserFriendlyException(message: "Phương thức này chỉ dùng để gửi tin nhắn cho người chưa có conversation!");

            var newConversation = await _conversationRepository.InsertAsync(new Conversation()
            {
                UserAId = currentUserId,
                UserBId = input.UserId,
                JustTwoPeople = true,
                IsActiveA = true,
                IsActiveB = false,
                IsStorageA = false,
                IsStorageB = false,
                LastIndexSeenA = 0,
                LastIndexSeenB = -1
            });

            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = input.Content,
                ConversationId = newConversation.Id,
                Index = 0,
                SenderId = currentUserId,
                Type = input.Type
            });

            newConversation.LastMessageId = newMessage.Id;

            // gửi thông báo đến người được nhận tin nhắn request
            await _hubContext.Clients.Group(input.UserId.ToString()).SendAsync("OnNewRequestMessage", newMessage);

            return new SendMessageToNewOtherResultDto()
            {
                NewConversation = ObjectMapper.Map<Conversation, ConversationDto>(newConversation),
                NewMessage = ObjectMapper.Map<Message, MessageDto>(newMessage)
            };
        }

        /// <summary>
        /// có sẵn conversation rồi mới dùng method này
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MessageDto> SendMessageToConversation(MessageSendToConversationDto input) 
        {
            var conversation = await _conversationRepository.GetAsync(input.ConversationId);

            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(input.ConversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }

            var lastMessage = await _messageRepository.GetAsync(conversation.LastMessageId.Value);

            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = input.Content,
                ConversationId = conversation.Id,
                SenderId = currentUserId,
                Type = input.Type,
                Index = lastMessage.Index + 1,
                FocusToMessageId = input.FocusToMessageId
            });

            conversation.LastMessageId = newMessage.Id;

            // cập nhật lastSeenIndex 
            // nếu conversation chưa active thì active
            if (conversation.JustTwoPeople.Value)
            {
                if (conversation.UserAId.Equals(currentUserId))
                {
                    conversation.LastIndexSeenA = newMessage.Index;
                    if (!conversation.IsActiveA.Value)
                        conversation.IsActiveA = true;
                }
                else
                {
                    conversation.LastIndexSeenB = newMessage.Index;
                    if (!conversation.IsActiveB.Value)
                        conversation.IsActiveB = true;
                }
            }
            else
            {
                var userConversation = await _userConversationRepository.GetAsync(x => x.ConversationId.Equals(conversation.Id) && x.UserId.Equals(currentUserId));
                userConversation.LastIndexSeen = newMessage.Index;

                if (!userConversation.IsActive.Value)
                    userConversation.IsActive = true;
            }
            await _conversationRepository.UpdateAsync(conversation);

            await _hubContext.Clients.Group(conversation.Id.ToString()).SendAsync("OnMessage", newMessage);

            return ObjectMapper.Map<Message, MessageDto>(newMessage);
        }

        public async Task<ConversationDto> AddConversation([FromForm] AddConversationDto input)
        {
            if (String.IsNullOrEmpty(input.AvatarUrl))
            {
                var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    await _blobContainer.SaveAsync(fileName, stream.ToArray(), overrideExisting: true);
                }

                var fileUrl = "https://fsiconnectedapi.azurewebsites.net/image/" + fileName;

                await _fileInfomationRepository.InsertAsync(new FileInfomation()
                {
                    AuthorId = this.currentUserId,
                    Url = fileUrl,
                    Size = (int)file.Length,
                    ContentType = file.ContentType
                });

                input.AvatarUrl = fileUrl;
            }

            var newConversation = await _conversationRepository.InsertAsync(new Conversation()
            {
                ConversationAvatar = input.AvatarUrl,
                ConversationName = input.ConversationName,
                JustTwoPeople = false,
            });

            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = "Tạo cuộc trò chuyện",
                Type = Common.Enums.MessageType.Text,
                Index = 0,
                SenderId = this.currentUserId,
                ConversationId = newConversation.Id,
            });

            newConversation.LastMessageId = newMessage.Id;

            var userConversations = new List<UserConversation>()
                    {
                        new UserConversation ()
                        {
                            ConversationId = newConversation.Id,
                            IsActive= true,
                            IsStorage = false,
                            IsDeleted= false,
                            LastIndexSeen = 0,
                            EnableNotification= true,
                            RoleInConversation = Common.Enums.UserConversationRole.Owner,
                            UserId = currentUserId
                        }
                    };

            var memberIds = JArray.Parse(input.MemberIds).ToObject<List<Guid>>();

            memberIds.Remove(currentUserId);

            memberIds.ForEach(userId =>
            {
                userConversations.Add(new UserConversation()
                {
                    ConversationId = newConversation.Id,
                    UserId = userId,
                    IsActive = false,
                    IsStorage = false,
                    IsDeleted = false,
                    LastIndexSeen = -1,
                    EnableNotification = true,
                    RoleInConversation = Common.Enums.UserConversationRole.Member
                });
            });

            await _userConversationRepository.InsertManyAsync(userConversations);

            // gửi thông báo đến tất cả những người được thêm
            await _hubContext.Clients.Groups(memberIds.Select(x => x.ToString()).ToList()).SendAsync("BeAddedToAConversation", newConversation);

            return ObjectMapper.Map<Conversation, ConversationDto>(newConversation);

        }

        public async Task<ConversationDto> PostUpdateConversation([FromForm] UpdateConversationDto input)
        {
            var conversation = await _conversationRepository.GetAsync(input.ConversationId);

            if (String.IsNullOrEmpty(input.AvatarUrl))
            {
                var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    await _blobContainer.SaveAsync(fileName, stream.ToArray(), overrideExisting: true);
                }

                var fileUrl = "https://fsiconnectedapi.azurewebsites.net/image/" + fileName;

                await _fileInfomationRepository.InsertAsync(new FileInfomation()
                {
                    AuthorId = this.currentUserId,
                    Url = fileUrl,
                    Size = (int)file.Length,
                    ContentType = file.ContentType
                });

                input.AvatarUrl = fileUrl;
            }

            conversation.ConversationAvatar = input.AvatarUrl;
            conversation.ConversationName = input.ConversationName;

            var lastMessage = await _messageRepository.GetAsync(conversation.LastMessageId.Value);

            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = "Thay đổi tên/avatar cuộc trò chuyện",
                ConversationId = conversation.Id,
                SenderId = currentUserId,
                Type = Common.Enums.MessageType.Text,
                Index = lastMessage.Index + 1
            });

            conversation.LastMessageId = newMessage.Id;
            var userConversation = await _userConversationRepository.GetAsync(x => x.ConversationId.Equals(conversation.Id) && x.UserId.Equals(currentUserId));
            userConversation.LastIndexSeen = newMessage.Index;

            await _hubContext.Clients.Group(conversation.Id.ToString()).SendAsync("OnMessage", newMessage);

            return ObjectMapper.Map<Conversation, ConversationDto>(conversation);
        }

        public async Task AddUserToConversation(Guid userId, Guid conversationId)
        {
            var userConversation = await _userConversationRepository.GetAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId) && x.IsActive.Value);

            if (userConversation == null)
            {
                throw new UserFriendlyException(message: "Bạn không có quyền thêm thành viên cho đoạn chat");
            }


            await _userConversationRepository.InsertAsync(new UserConversation()
            {
                ConversationId = conversationId,
                UserId = userId,
                EnableNotification = true,
                IsActive = false,
                IsStorage = false,
                IsDeleted = false,
                RoleInConversation = Common.Enums.UserConversationRole.Member,
                LastIndexSeen = -1
            });

            var conversation = await _conversationRepository.GetAsync(conversationId);
            var lastMessage = await _messageRepository.GetAsync(conversation.LastMessageId.Value);
            var user = await _userRepository.GetAsync(userId);
            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = $"Thêm {user.Name} vào cuộc hội thoại",
                ConversationId = conversation.Id,
                SenderId = currentUserId,
                Type = Common.Enums.MessageType.Text,
                Index = lastMessage.Index + 1
            });
            conversation.LastMessageId = newMessage.Id;
            userConversation.LastIndexSeen = newMessage.Index;

            await _hubContext.Clients.Group(userId.ToString()).SendAsync("BeAddedToAConversation", conversationId);
            await _hubContext.Clients.Group(conversation.Id.ToString()).SendAsync("OnMessage", newMessage);
        }

        public async Task RemoveUserFromConversation(Guid userId, Guid conversationId)
        {
            var userConversation = await _userConversationRepository.GetAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId) && x.IsActive.Value);

            if (userConversation == null)
            {
                throw new UserFriendlyException(message: "Bạn không có quyền xóa thành viên khỏi đoạn chat");
            }

            var _userConversation = await _userConversationRepository.GetAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(userId));
            if (userConversation.RoleInConversation > _userConversation.RoleInConversation)
                throw new UserFriendlyException(message: "Bạn không có quyền xóa thành viên khỏi đoạn chat");

            await _userConversationRepository.DeleteAsync(_userConversation);

            var conversation = await _conversationRepository.GetAsync(conversationId);
            var lastMessage = await _messageRepository.GetAsync(conversation.LastMessageId.Value);
            var user = await _userRepository.GetAsync(userId);
            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = $"Xóa {user.Name} khỏi cuộc hội thoại",
                ConversationId = conversation.Id,
                SenderId = currentUserId,
                Type = Common.Enums.MessageType.Text,
                Index = lastMessage.Index + 1
            });
            conversation.LastMessageId = newMessage.Id;
            userConversation.LastIndexSeen = newMessage.Index;
            await _hubContext.Clients.Group(conversation.Id.ToString()).SendAsync("OnMessage", newMessage);
        }

        public async Task AcceptPendingConversation(Guid conversationId)
        {
            var conversation = await _conversationRepository.GetAsync(conversationId);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
                else if ((conversation.UserAId.Equals(currentUserId) && conversation.IsActiveA.Value) || (conversation.UserBId.Equals(currentUserId) && conversation.IsActiveB.Value))
                    throw new UserFriendlyException(message: "Conversation đã active từ trước");

                if (conversation.UserAId.Equals(currentUserId))
                {
                    conversation.IsActiveA = true;
                }
                else
                {
                    conversation.IsActiveB = true;
                }
                await _conversationRepository.UpdateAsync(conversation);
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
                else if (userConversation.IsActive.Value)
                    throw new UserFriendlyException(message: "Conversation đã active từ trước");

                userConversation.IsActive = true;
                await _userConversationRepository.UpdateAsync(userConversation);
            }
        }

        public async Task SeenConversation(Guid conversationId)
        {
            var conversation = await _conversationRepository.GetAsync(conversationId);
            var lastMessage = await _messageRepository.GetAsync(conversation.LastMessageId.Value);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");

                if (conversation.UserAId.Equals(currentUserId))
                {
                    conversation.LastIndexSeenA = lastMessage.Index;
                }
                else
                {
                    conversation.LastIndexSeenB = lastMessage.Index;
                }
                await _conversationRepository.UpdateAsync(conversation);
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");

                userConversation.LastIndexSeen = lastMessage.Index;
                await _userConversationRepository.UpdateAsync(userConversation);
            }
        }

        public async Task TestSignalR()
        {
            await _hubContext.Clients.Group(currentUserId.ToString()).SendAsync("OnTestHehe", "Nguyễn Văn Hiếu");
        }

        public async Task<ConversationDto> GetConversationByUserId(Guid userId)
        {
            var conversation = await _conversationRepository.FindAsync(x => (x.UserAId.Equals(currentUserId) && x.UserBId.Equals(userId)) ||
                                                                           (x.UserBId.Equals(currentUserId) && x.UserAId.Equals(userId)));

            var userPatner = await _userRepository.GetAsync(userId);


            if (conversation == null)
            {
                conversation = new Conversation()
                {
                    ConversationName = userPatner.Name,
                    ConversationAvatar = userPatner.AvatarUrl
                };
            }
            else
            {
                conversation.ConversationName = userPatner.Name;
                conversation.ConversationAvatar = userPatner.AvatarUrl;
            }


            return ObjectMapper.Map<Conversation, ConversationDto>(conversation);

        }

        public async Task<List<UserConversationDto>> GetUsersByConversation(Guid conversationId)
        {
            var users = await _userRepository.GetListAsync();
            var userConversations = await _userConversationRepository.GetListAsync(x => x.ConversationId.Equals(conversationId));
            return ObjectMapper.Map<List<UserConversation>, List<UserConversationDto>>(userConversations);
        }

        public async Task PostReactMessage(PostReactMessageDto input)
        {
            var message = await _messageRepository.GetAsync(input.MessageId);

            var conversation = await _conversationRepository.GetAsync(message.ConversationId);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(message.ConversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }

            var oldReact = message.Reacts.FirstOrDefault(x => x.UserId == currentUserId);

            if (oldReact == null)
            {
                if (input.React != null)
                {
                    message.Reacts.Add(new UserReactMessage()
                    {
                        React = input.React.Value,
                        UserId = currentUserId
                    });
                }
                else return;
            }
            else
            {
                if (input.React != null)
                {
                    oldReact.React = input.React.Value;
                }
                else
                {
                    message.Reacts.Remove(oldReact);
                }
            }

            await _messageRepository.UpdateAsync(message);

            await _hubContext.Clients.Group(message.ConversationId.ToString()).SendAsync("OnReactMessage", message);
        }

        public async Task PostPinMessage(PostPinMessageDto input)
        {
            var message = await _messageRepository.GetAsync(input.MessageId);

            var conversation = await _conversationRepository.GetAsync(message.ConversationId);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(message.ConversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }

            message.IsPinned = input.IsPin;
            await _messageRepository.UpdateAsync(message);

            await _hubContext.Clients.Group(message.ConversationId.ToString()).SendAsync("OnPinMessage", message);
        }

        public async Task<List<MessageDto>> GetListPinMessageByConversation(Guid conversationId)
        {
            var conversation = await _conversationRepository.GetAsync(conversationId);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }

            var pinnedMessages = await _messageRepository.GetListAsync(x => x.ConversationId == conversationId && x.IsPinned);

            return ObjectMapper.Map<List<Message>, List<MessageDto>>(pinnedMessages);
        }

        public async Task DeleteMessage(Guid messageId)
        {
            var message = await _messageRepository.GetAsync(messageId);

            if (message.SenderId != currentUserId)
                throw new UserFriendlyException(message: "Bạn không thể xóa tin nhắn của người khác!");

            var conversation = await _conversationRepository.GetAsync(message.ConversationId);

            if(conversation.LastMessageId == messageId)
            {
                var messages = await _messageRepository.GetListAsync(x => x.ConversationId == message.ConversationId);
                var previousMessage = messages.OrderByDescending(x=> x.Index).ElementAt(1);
                conversation.LastMessageId = previousMessage.Id;
                await _conversationRepository.UpdateAsync(conversation);
            }

            await _messageRepository.DeleteAsync(messageId);

            await _hubContext.Clients.Group(message.ConversationId.ToString()).SendAsync("OnDeleteMessage", message);
        }

        public async Task CreateMeetInviteKey(Guid conversationId)
        {
            var conversation = await _conversationRepository.GetAsync(conversationId);
            if (conversation.JustTwoPeople.Value)
            {
                if (!conversation.UserAId.Equals(currentUserId) && !conversation.UserBId.Equals(currentUserId))
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }
            else
            {
                var userConversation = await _userConversationRepository.FindAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId));
                if (userConversation == null)
                    throw new UserFriendlyException(message: "Bạn không thuộc về đoạn hội thoại này!");
            }

            var inviteKey = Guid.NewGuid().ToString();


        }
    }
}
