using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Chat
{
    public class Conversation : FullAuditedAggregateRoot<Guid>
    {
        public bool JustTwoPeople { get; set; }

        public string ConversationName { get; set; }

        public string ConversationAvatar { get; set; }

        public string Tag { get; set; }

        public string JoinLink { get; set; }

        public Guid LastMessageId { get; set; }

    }
}
