using FSI.Application.Contracts.Chat.DTO;
using FSI.Application.Contracts.Chat.IService;
using FSI.Application.Contracts.CommonDto;
using FSI.Application.Hubs;
using FSI.Domain.Chat;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly Guid currentUserId;

        public ChatAppService(IConversationRepository conversationRepository, IUserConversationRepository userConversationRepository, IMessageRepository messageRepository, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository)
        {
            _conversationRepository = conversationRepository;
            _userConversationRepository = userConversationRepository;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<ConversationDto>> GetListConversation(GetListConversationDto input)
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
            var pagedConversations = myconversations.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            // lấy lastMessage
            pagedConversations = pagedConversations.Join(messages, x => x.LastMessageId, y => y.Id, (x, y) =>
            {
                x.LastMessage = y;
                return x;
            }).ToList();

            return new PagedResultDto<ConversationDto>()
            {
                Items = ObjectMapper.Map<List<Conversation>, List<ConversationDto>>(pagedConversations),
                TotalCount = myconversations.Count
            };

        }

        public async Task<PagedResultDto<MessageDto>> GetListMessageByConversation(GetListMessageDto input)
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

            return new PagedResultDto<MessageDto>()
            {
                Items = ObjectMapper.Map<List<Message>, List<MessageDto>>(messages.Skip(input.SkipCount).Take(input.MaxResultCount).OrderByDescending(x => x.CreationTime).ToList()),
                TotalCount = messages.Count
            };
        }

        /// <summary>
        /// Chỉ dùng để nhắn tin cho người lạ ( chưa có conversation)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<MessageDto> SendMessageToNewOther(MessageSendToUserDto input)
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
                IsStorageB = false
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
            await _conversationRepository.UpdateAsync(newConversation);

            // gửi thông báo đến người được nhận tin nhắn request
            await _hubContext.Clients.Group(input.UserId.ToString()).SendAsync("OnNewRequestMessage", newMessage);

            return ObjectMapper.Map<Message, MessageDto>(newMessage);
        }

        /// <summary>
        /// có sẵn conversation rồi mới dùng method này
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MessageDto> SendMessageToConversation(MessageSendToConversationDto input)
        {
            var conversation = await _conversationRepository.GetAsync(input.ConversationId);

            var lastMessage = await _messageRepository.GetAsync(conversation.LastMessageId.Value);

            var newMessage = await _messageRepository.InsertAsync(new Message()
            {
                Content = input.Content,
                ConversationId = conversation.Id,
                SenderId = currentUserId,
                Type = input.Type,
                Index = lastMessage.Index + 1
            });

            conversation.LastMessageId = newMessage.Id;
            await _conversationRepository.UpdateAsync(conversation);

            await _hubContext.Clients.Group(conversation.Id.ToString()).SendAsync("OnMessage", newMessage);

            return ObjectMapper.Map<Message, MessageDto>(newMessage);
        }

        public async Task<ConversationDto> AddConversation(AddConversationDto input)
        {
            var newConversation = await _conversationRepository.InsertAsync(new Conversation()
            {
                ConversationName = input.ConversationName,
                JustTwoPeople = false,
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

            return ObjectMapper.Map<Conversation, ConversationDto>(newConversation);

        }

        public async Task AddUserToConversation(Guid userId, Guid conversationId)
        {
            var userConversation = await _userConversationRepository.GetAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(currentUserId) && x.IsActive.Value);
            
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
        }
    }
}
