using FSI.Application.Hubs;
using FSI.Domain.Account;
using FSI.Domain.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Agora
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class MeetHub : Hub
    {
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly MeetHubService _meetHubService;

        public MeetHub(IHttpContextAccessor httpContextAccessor, MeetHubService meetHubService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _meetHubService = meetHubService;
        }

        public async Task JoinMeet(Guid conversationId)
        {
            var currentUserMeet = _meetHubService.UserMeets.FirstOrDefault(x => x.Uid == currentUserId);

            if (currentUserMeet != null)
            {
                await Clients.Caller.SendAsync("OnJoinFailed", "Bạn chỉ có thể tham gia 1 meet trong 1 thời điểm. Hãy thoát các meet khác trước khi tham gia meet này!");
            }
            else
            {
                _meetHubService.UserMeets.Add(new UserMeet()
                {
                    ConversationId = conversationId,
                    ConnectionId = Context.ConnectionId,
                    Uid = currentUserId,
                    Micro = false,
                    Screen = false,
                    Video = false
                });
                await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
                await Clients.Caller.SendAsync("OnJoinSuccess");
                await Clients.Group(conversationId.ToString()).SendAsync("OnJoined", currentUserId);
            }
        }

        public async Task Chat(string message)
        {
            var userMeet = _meetHubService.UserMeets.FirstOrDefault(x => x.Uid == currentUserId);
            await Clients.Group(userMeet.ConversationId.ToString()).SendAsync("OnMessage", currentUserId, message);
        }

        public async Task Publish(string track)
        {
            var userMeet = _meetHubService.UserMeets.FirstOrDefault(x => x.Uid == currentUserId);
            switch (track)
            {
                case "micro":
                    userMeet.Micro = true;
                    break;
                case "video":
                    userMeet.Video = true;
                    break;
                case "screen":
                    userMeet.Screen = true;
                    break;
            }
            await Clients.Groups(userMeet.ConversationId.ToString()).SendAsync("OnPublish", currentUserId, track);
        }

        public async Task UnPublish(string track)
        {
            var userMeet = _meetHubService.UserMeets.FirstOrDefault(x => x.Uid == currentUserId);
            switch (track)
            {
                case "micro":
                    userMeet.Micro = false;
                    break;
                case "video":
                    userMeet.Video = false;
                    break;
                case "screen":
                    userMeet.Screen = false;
                    break;
            }
            await Clients.Groups(userMeet.ConversationId.ToString()).SendAsync("OnUnPublish", currentUserId, track);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userMeet = _meetHubService.UserMeets.FirstOrDefault(x => x.Uid == currentUserId && x.ConnectionId == Context.ConnectionId);
            if(userMeet != null)
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, userMeet.ConversationId.ToString());
                Clients.Groups(userMeet.ConversationId.ToString()).SendAsync("OnLeave", currentUserId);
                _meetHubService.UserMeets.Remove(userMeet);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }

}
