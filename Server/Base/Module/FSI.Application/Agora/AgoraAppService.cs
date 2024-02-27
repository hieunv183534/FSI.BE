using AgoraIO.Media;
using FSI.Application.Contracts.Agora.DTO;
using FSI.Application.Contracts.Agora.IService;
using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Hubs;
using FSI.Domain.Chat;
using FSI.Domain.User;
using FSI.Lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace FSI.Application.Agora
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class AgoraAppService : ApplicationService, IAgoraAppService
    {

        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IConversationRepository _conversationRepository;
        private readonly IUserConversationRepository _userConversationRepository;
        private readonly IConfiguration Configuration;

        private readonly IHubContext<ChatHub> _hubContext;
        private readonly MeetHubService _meetHubService;


        public AgoraAppService(IHttpContextAccessor httpContextAccessor, IConversationRepository conversationRepository, IUserConversationRepository userConversationRepository, IConfiguration configuration, IHubContext<ChatHub> hubContext, MeetHubService meetHubService)
        {
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _conversationRepository = conversationRepository;
            _userConversationRepository = userConversationRepository;
            Configuration = configuration;
            _hubContext = hubContext;
            _meetHubService = meetHubService;
        }

        public async Task<string> CreateRtcToken(GetTokenDto input)
        {
            var uIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserId = Guid.Parse(uIdStr);
            var conversationId = Guid.Parse(input.ChannelName);

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


            var token1 = new AccessToken(Configuration["Agora:AppId"],
                                        Configuration["Agora:AppCertificate"],
                                        input.ChannelName,
                                        uIdStr);
            token1.addPrivilege(Privileges.kJoinChannel, DateTime.Now.AddDays(1).ToDoUInt32DateTime());
            string result1 = token1.build();

            var token2 = new AccessToken(Configuration["Agora:AppId"],
                                        Configuration["Agora:AppCertificate"],
                                        input.ChannelName,
                                        uIdStr + "screen");
            token2.addPrivilege(Privileges.kJoinChannel, DateTime.Now.AddDays(1).ToDoUInt32DateTime());
            string result2 = token2.build();

            return result1 + "_and_" + result2;
        }

        [AllowAnonymous]
        public async Task<string> LoginAsGuestToMeet(GuestToMeetDto input)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("this-is-my-super-key");

            var guestId = Guid.NewGuid;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("guestName", input.GuestName),
                        new Claim("nameid", guestId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
