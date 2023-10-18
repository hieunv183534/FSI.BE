using AgoraIO.Media;
using FSI.Application.Contracts.Agora.DTO;
using FSI.Application.Contracts.Agora.IService;
using FSI.Domain.Chat;
using FSI.Domain.User;
using FSI.Lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IUserConversationRepository _userConversationRepository;

        public AgoraAppService(IHttpContextAccessor httpContextAccessor, IConversationRepository conversationRepository, IUserConversationRepository userConversationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _conversationRepository = conversationRepository;
            _userConversationRepository = userConversationRepository;
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


            var token = new AccessToken("48f5a9f8d4e644a6a1ca96376fdcf441",
                                        "cfbf074b9424427bba74f3c47b998921",
                                        input.ChannelName,
                                        uIdStr);

            token.addPrivilege(Privileges.kJoinChannel, DateTime.Now.AddDays(1).ToDoUInt32DateTime());
            string result = token.build();
            return result;
        }
    }
}
