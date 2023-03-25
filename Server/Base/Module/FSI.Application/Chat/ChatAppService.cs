using FSI.Application.Contracts.Chat.DTO;
using FSI.Application.Contracts.Chat.IService;
using FSI.Application.Contracts.CommonDto;
using FSI.Application.Hubs;
using FSI.Domain.Chat;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace FSI.Application.Chat
{
    [Authorize]
    public class ChatAppService : ApplicationService, IChatAppService
    {
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserConversationRepository _userConversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly Guid currentUserId;

        public ChatAppService(IConversationRepository conversationRepository, IUserConversationRepository userConversationRepository, IMessageRepository messageRepository, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _conversationRepository = conversationRepository;
            _userConversationRepository = userConversationRepository;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        /// <summary>
        /// thêm hội thoại
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> AddConversation(AddConversationDto input)
        {
            var newConversation = await _conversationRepository.InsertAsync(new Conversation()
            {
                ConversationName = input.ConversationName,
                JustTwoPeople = false
            });

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

            input.MemberIds.ForEach(userId =>
            {
                userConversations.Add(new UserConversation()
                {
                    ConversationId = newConversation.Id,
                    UserId = userId,
                    IsActive = false,
                    IsStorage = false,
                    IsDeleted = false,
                    LastIndexSeen = 0,
                    EnableNotification = true,
                    RoleInConversation = Common.Enums.UserConversationRole.Member
                });
            });

            await _userConversationRepository.InsertManyAsync(userConversations);

            // gửi thông báo đến tất cả những người được thêm
            await _hubContext.Clients.Groups(input.MemberIds.Select(x => x.ToString()).ToList()).SendAsync("BeAddedToAConversation", newConversation);

            return new ServiceResult()
            {
                Code = 20000,
                Message = "Add conversation successfully!",
                Data = newConversation
            };
        }

        public async Task<ServiceResult> AddUserToConversation(Guid userId, Guid conversationId)
        {
            var userConversation = await _userConversationRepository.FirstOrDefaultAsync(uc => uc.UserId.Equals(currentUserId) && uc.ConversationId.Equals(conversationId));

            if (userConversation == null)
            {
                return new ServiceResult()
                {
                    Code = 40301,
                    Message = "You not in this Conversation!"
                };
            }
            else if (userConversation.RoleInConversation == Common.Enums.UserConversationRole.Owner ||
                userConversation.RoleInConversation == Common.Enums.UserConversationRole.Admin)
            {
                // khi một người add người khác vào conversation thì add vào db và ở dạng chưa active
                await _userConversationRepository.InsertAsync(new UserConversation()
                {
                    ConversationId = conversationId,
                    UserId = userId,
                    EnableNotification = false,
                    IsActive = false,
                    RoleInConversation = Common.Enums.UserConversationRole.Member,
                    LastIndexSeen = 0
                });

                await _hubContext.Clients.Group(userId.ToString()).SendAsync("BeAddedToAConversation", conversationId);

                return new ServiceResult()
                {
                    Code = 20000,
                    Message = "Add user to conversation successfully.Wait for the user to accept"
                };
            }
            else
            {
                return new ServiceResult()
                {
                    Code = 40302,
                    Message = "You are not an admmin of this Conversation!"
                };
            }
        }

        public async Task<ServiceResult> GetListConversation(GetListConversationDto input)
        {


            var conversations = await _conversationRepository.GetQueryableAsync();
            var userConversations = await _userConversationRepository.GetQueryableAsync();

            // lấy các conversation hai người 
            var conversation1s = conversations.Where(c => c.JustTwoPeople &&
            (c.UserAId.Equals(currentUserId) || c.UserAId.Equals(currentUserId)) &&
            c.ConversationName.Contains(input.Filter)).ToList();

            // lấy các conversation còn lại
            var conversation2s = (from c in conversations
                                  join uc in userConversations
                                  on c.Id equals uc.ConversationId
                                  where uc.UserId.Equals(currentUserId) &&
                                  c.ConversationName.Contains(input.Filter)
                                  select c).ToList();
            return new ServiceResult()
            {
                Code = 20000,
                Message = "Get list conversation successfully!",
                Data = conversation1s
                        .Concat(conversation2s)
                        .OrderBy(c => c.LastMessage.CreationTime)
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount)
            };
        }

        public async Task<ServiceResult> SendMessageToUser(MessageSendToUserDto message)
        {
            var oldConversation = await _conversationRepository.FindAsync(c => (c.UserAId.Equals(currentUserId) && c.UserBId.Equals(message.TargetId)) ||
                                                                            (c.UserBId.Equals(currentUserId) && c.UserAId.Equals(message.TargetId)));
            Message newMessage;
            
            if (oldConversation == null) // trường hợp tin nhắn request đầu tiên
            {
                var newConversation = await _conversationRepository.InsertAsync(new Conversation()
                {
                    UserAId = currentUserId,
                    UserBId = message.TargetId,
                    JustTwoPeople = true,
                });

                newMessage = await _messageRepository.InsertAsync(new Message()
                {
                    Content = message.Content,
                    ConversationId = newConversation.Id,
                    Index = 0,
                    SenderId = currentUserId,
                    Type = message.Type,
                });
                newConversation.LastMessageId = newMessage.Id;
                await _conversationRepository.UpdateAsync(newConversation);


                // gửi thông báo đến người được nhận tin nhắn request
                await _hubContext.Clients.Group(message.TargetId.ToString()).SendAsync("OnNewRequestMessage", newMessage);
            }
            else
            {
                newMessage = await _messageRepository.InsertAsync(new Message()
                {
                    Content = message.Content,
                    ConversationId = oldConversation.Id,
                    Index = oldConversation.LastMessage.Index++,
                    SenderId = currentUserId,
                    Type = message.Type,
                });
                oldConversation.LastMessageId = newMessage.Id;
                await _conversationRepository.UpdateAsync(oldConversation);

                await _hubContext.Clients.Group(oldConversation.Id.ToString()).SendAsync("OnMessage", newMessage);
            }

            return new ServiceResult()
            {
                Code = 20000,
                Message = "Send message to user successfully",
                Data = newMessage
            };
        }

        public async Task<ServiceResult> SendMessageToConversation(MessageSendToConversationDto message)
        {
            var conversation = await _conversationRepository.GetAsync(message.ConversationId);

            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = message.Content,
                ConversationId = conversation.Id,
                SenderId = currentUserId,
                Type = message.Type,
                Index = conversation.LastMessage.Index++
            });

            conversation.LastMessageId = newMessage.Id;
            await _conversationRepository.UpdateAsync(conversation);

            await _hubContext.Clients.Group(conversation.Id.ToString()).SendAsync("OnMessage", newMessage);

            return new ServiceResult()
            {
                Code = 20000,
                Message = "Send message to conversation successfullly",
                Data = newMessage
            };
        }

        public async Task<ServiceResult> SetNickName(SetNickNameDto input)
        {
            var conversation = await _conversationRepository.GetAsync(input.ConversationId);

            if (conversation == null)
                return new ServiceResult()
                {
                    Code = 40001,
                    Message = "Conversation not found!"
                };

            var myConversation = await _userConversationRepository.FindAsync(uc => uc.ConversationId.Equals(input.ConversationId) && uc.UserId.Equals(currentUserId));

            if (myConversation == null)
                return new ServiceResult()
                {
                    Code = 40103,
                    Message = "Conversation not contain you!"
                };

            var userConversation = await _userConversationRepository.FindAsync(uc => uc.ConversationId.Equals(input.ConversationId) && uc.UserId.Equals(input.UserId));

            if (userConversation == null)
                return new ServiceResult()
                {
                    Code = 40103,
                    Message = "Conversation not contain this user!"
                };

            if (myConversation.RoleInConversation <= userConversation.RoleInConversation)
            {
                userConversation.NickName = input.NickName;
                await _userConversationRepository.UpdateAsync(userConversation);
                return new ServiceResult()
                {
                    Code = 20000,
                    Message = "Set nickname successfully"
                };
            }
            else
            {
                return new ServiceResult()
                {
                    Code = 40303,
                    Message = "You don't have enoungh role!"
                };
            }
        }
    }
}
