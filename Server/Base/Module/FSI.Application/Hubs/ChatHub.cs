using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Chat;
using FSI.Domain.Test;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserConversationRepository _userConversationRepository;
        private Guid currentUserId;

        public static List<UserConnection> Connections { get; set; } = new List<UserConnection>();

        public ChatHub(IObjectMapper objectMapper, IUnitOfWorkManager unitOfWorkManager, IUserConversationRepository userConversationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
            _userConversationRepository = userConversationRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
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

                // thêm userConnection này
                Connections.Add(new UserConnection()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = currentUserId,
                });

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

                //xóa userConnection này
                var userConnection = Connections.FirstOrDefault(x=> x.ConnectionId == Context.ConnectionId && x.UserId == currentUserId);
                Connections.Remove(userConnection);

                await uow.CompleteAsync();
            }
        }
    }
}
