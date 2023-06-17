using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Chat;
using FSI.Domain.Test;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace FSI.Application.Hubs
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ChatHub : Hub
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserConversationRepository _userConversationRepository;
        private Guid currentUserId;

        public ChatHub(IObjectMapper objectMapper, IUnitOfWorkManager unitOfWorkManager, IUserConversationRepository userConversationRepository)
        {
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _userConversationRepository = userConversationRepository;
            this.currentUserId = Guid.Parse(Context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public override async Task OnConnectedAsync()
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {

                // lấy ra tất cả conversation thuộc về người dùng rồi add người dùng vào tất cả groupConversation đấy
                var conversationsOfUser = await _userConversationRepository.GetListAsync(uc => uc.UserId.Equals(currentUserId));
                conversationsOfUser.ForEach(c =>
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, c.Id.ToString());
                });


                // add connection vào group idUser hiện tại dùng để send cho riêng người dùng này
                await Groups.AddToGroupAsync(Context.ConnectionId, currentUserId.ToString());

                await uow.CompleteAsync();
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                // lấy ra tất cả conversation thuộc về người dùng rồi xóa connection từ tất cả groupConversation đấy
                var conversationsOfUser = await _userConversationRepository.GetListAsync(uc => uc.UserId.Equals(currentUserId));
                conversationsOfUser.ForEach(c =>
                {
                    Groups.RemoveFromGroupAsync(Context.ConnectionId, c.Id.ToString());
                });

                // xóa connection từ group idUser hiện tại dùng để send cho riêng người dùng này
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentUserId.ToString());
                await uow.CompleteAsync();
            }
        }
    }
}
