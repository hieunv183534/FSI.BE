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
            await Clients.Group(conversationId.ToString()).SendAsync("OnJoined", currentUserId);
        }

        public async Task Chat(Guid conversationId, string message)
        {
            await Clients.Group(conversationId.ToString()).SendAsync("OnMessage", currentUserId, message);
        }

        public async Task Publish(Guid conversationId, string track)
        {
            var userMeet = _meetHubService.UserMeets.FirstOrDefault(x=> x.ConversationId == conversationId && x.Uid == currentUserId);
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
            await Clients.Groups(conversationId.ToString()).SendAsync("OnPublish", currentUserId, track);
        }

        public async Task UnPublish(Guid conversationId, string track)
        {
            var userMeet = _meetHubService.UserMeets.FirstOrDefault(x => x.ConversationId == conversationId && x.Uid == currentUserId);
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
            await Clients.Groups(conversationId.ToString()).SendAsync("OnUnPublish", currentUserId, track);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var currentUserMeets = _meetHubService.UserMeets.Where(x => x.Uid == currentUserId).ToList();
            currentUserMeets.ForEach(um =>
            {
                _meetHubService.UserMeets.Remove(um);
            });
            await Clients.Group()
            return base.OnDisconnectedAsync(exception);
        }
    }

}
