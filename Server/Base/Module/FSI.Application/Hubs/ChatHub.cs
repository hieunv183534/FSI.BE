using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Chat;
using FSI.Domain.Test;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace FSI.Application.Hubs
{
    [Authorize]
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
                await uow.CompleteAsync();
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var userConnection = await _userConnectionRepository.GetAsync(uc => uc.ConnectionId.Equals(Context.ConnectionId));
                await _userConnectionRepository.DeleteAsync(userConnection.Id);
                await uow.CompleteAsync();
            }
        }
    }
}
