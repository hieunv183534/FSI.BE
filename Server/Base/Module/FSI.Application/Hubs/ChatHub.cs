using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Chat;
using FSI.Domain.Test;
using FSI.Domain.User;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace FSI.Application.Hubs
{
    
    public class ChatHub : Hub
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserConversationRepository _userConversationRepository;
        private readonly IUserConnectionRepository _userConnectionRepository;

        public ChatHub(IObjectMapper objectMapper, IUnitOfWorkManager unitOfWorkManager, IUserConnectionRepository userConnectionRepository, IUserConversationRepository userConversationRepository)
        {
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _userConnectionRepository = userConnectionRepository;
            _userConversationRepository = userConversationRepository;
        }

        public override async Task OnConnectedAsync()
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var userId = Guid.Parse(Context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // lấy ra tất cả conversation thuộc về người dùng rồi add người dùng vào tất cả groupConversation đấy

                var conversationsOfUser = await _userConversationRepository.GetListAsync(uc => uc.UserId.Equals(userId));

                conversationsOfUser.ForEach(c =>
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, c.Id.ToString());
                });

                await _userConnectionRepository.InsertAsync(new UserConnection()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userId
                });
            }
        }


        public async Task AddUserToConversation(Guid userId, Guid conversationId)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var currentUserId = Guid.Parse(Context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var userConversation = await _userConversationRepository.FirstOrDefaultAsync(uc => uc.UserId.Equals(currentUserId) && uc.ConversationId.Equals(conversationId));

                if(userConversation == null)
                {
                    await Clients.Caller.SendAsync("ResultAddUserToConversation", 4031, "You not in this Conversation!");
                }
                else if(userConversation.RoleInConversation == Common.Enums.UserConversationRole.Owner ||
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

                    // lấy connections của người đó để thông báo về việc add này.
                    // lấy hết các conncectionId tương ứng với các connection người đó ở các thiết bị khác nhau
                    var connectionIds = (await _userConnectionRepository.GetListAsync(uc => uc.UserId.Equals(userId))).Select(uc => uc.ConnectionId);
                    await Clients.Clients(connectionIds.ToList()).SendAsync("BeAddedToAConversation", conversationId);
                }
                else
                {
                    await Clients.Caller.SendAsync("ResultAddUserToConversation", 4032, "You are not admin of this Conversation");
                }
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var userConnection = await _userConnectionRepository.GetAsync(uc => uc.ConnectionId.Equals(Context.ConnectionId));
                await _userConnectionRepository.DeleteAsync(userConnection.Id);
            }
        }
    }
}
