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

        Task RemoveUserFromConversation(Guid userId, Guid conversationId);

        Task<ConversationDto> AddConversation(AddConversationDto input);

        Task<ConversationDto> PostUpdateConversation(UpdateConversationDto input);

        Task<PagedResultDto<ConversationDto>> PostToGetListConversation(GetListConversationDto input);

        Task<PagedResultDto<MessageDto>> PostToGetListMessageByConversation(GetListMessageDto input);

        Task<SendMessageToNewOtherResultDto> SendMessageToNewOther(MessageSendToUserDto input);

        Task<MessageDto> SendMessageToConversation(MessageSendToConversationDto message);

        Task AcceptPendingConversation(Guid conversationId);

        Task SeenConversation(Guid conversationId);

        Task TestSignalR();

        Task<ConversationDto> GetConversationByUserId(Guid userId);

        Task<List<UserConversationDto>> GetUsersByConversation(Guid conversationId);

        Task PostReactMessage(PostReactMessageDto input);

        Task PostPinMessage(PostPinMessageDto input);

        Task<List<MessageDto>> GetListPinMessageByConversation(Guid conversationId);

        Task DeleteMessage(Guid messageId);

        Task CreateMeetInviteKey(CreateMeetInviteKeyDto input);

        Task<List<MeetInviteKeyDto>> GetListInviteKeys(Guid conversationId);

        Task DeleteInviteKey(Guid conversationId, string key);
    }
}
