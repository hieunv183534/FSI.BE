using FSI.Application.Contracts.Chat.DTO;
using FSI.Application.Contracts.CommonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.IService
{
    public interface IChatAppService
    {
        public Task<ServiceResult> AddUserToConversation(Guid userId, Guid conversationId);

        public Task<ServiceResult> AddConversation(AddConversationDto input);

        public Task<ServiceResult> GetListConversation(GetListConversationDto input);

        public Task<ServiceResult> SendMessageToUser(MessageSendToUserDto message);

        public Task<ServiceResult> SendMessageToConversation(MessageSendToConversationDto message);
    }
}
