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
    public class UserConversationDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid ConversationId { get; set; }

        public ConversationDto Conversation { get; set; }

        public Guid UserId { get; set; }

        public UserRootDto User { get; set; }

        public UserConversationRole? RoleInConversation { get; set; }

        public string? NickName { get; set; }

        public int? LastIndexSeen { get; set; }

        public bool? IsActive { get; set; }

        public bool? EnableNotification { get; set; }

        public DateTime? OffNotificationTo { get; set; }

        public bool? IsStorage { get; set; }
    }
}
