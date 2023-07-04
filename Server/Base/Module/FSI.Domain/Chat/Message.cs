using FSI.Common.Enums;
using FSI.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Chat
{
    public class Message : FullAuditedAggregateRoot<Guid>
    {
        public Guid ConversationId { get; set; }

        public Conversation Conversation { get; set; }

        public Guid SenderId { get; set; }

        public UserRoot Sender { get; set; }

        public int? Index { get; set; }

        public MessageType? Type { get; set; }

        public string? Content { get; set; }

        public Guid? FocusToMessageId { get; set; }

    }
}
