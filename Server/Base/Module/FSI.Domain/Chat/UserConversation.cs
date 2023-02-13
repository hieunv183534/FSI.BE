using FSI.Common.Enums;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Chat
{
    public class UserConversation : FullAuditedAggregateRoot<Guid>
    {
        public Guid ConversationId { get; set; }

        public Conversation Conversation { get; set; }

        public Guid UserId { get; set; }

        public UserRoot User { get; set; }

        public UserConversationRole RoleInConversation { get; set; }

        public string NickName { get; set; }

        public int LastIndexSeen { get; set; }

        public bool IsActive { get; set; }

        public bool EnableNotification { get; set; }

        public DateTime OffNotificationTo { get; set; }

        public bool IsStorage { get; set; }

    }
}
