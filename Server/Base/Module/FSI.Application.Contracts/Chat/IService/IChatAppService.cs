using FSI.Application.Contracts.Chat.DTO;
using FSI.Application.Contracts.CommonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Chat.IService
{
    public interface IChatAppService
    {
        Task AddUserToConversation(Guid userId, Guid conversationId);

        Task<ConversationDto> AddConversation(AddConversationDto input);

        Task<PagedResultDto<ConversationDto>> GetListConversation(GetListConversationDto input);

        Task<PagedResultDto<MessageDto>> GetListMessageByConversation(GetListMessageDto input);

        Task<MessageDto> SendMessageToNewOther(MessageSendToUserDto input);

        Task<MessageDto> SendMessageToConversation(MessageSendToConversationDto message);


        //public Task<ServiceResult> SendMessageToUser(MessageSendToUserDto message);


        //public Task<ServiceResult> SetNickName(SetNickNameDto input);
    }
}
