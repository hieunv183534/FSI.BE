using FSI.Application.Contracts.User.DTO;
using FSI.Common.Enums;
using FSI.Domain.Chat;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class MessageDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid ConversationId { get; set; }

        public ConversationDto Conversation { get; set; }

        public Guid SenderId { get; set; }

        public UserRootDto Sender { get; set; }

        public int? Index { get; set; }

        public MessageType? Type { get; set; }

        public string? Content { get; set; }

        public Guid? FocusToMessageId { get; set; }

        public MessageDto FocusToMessage { get; set; }

        public bool? IsMine { get; set; }

        public List<UserReactMessage> Reacts { get; set; }
    }
}
