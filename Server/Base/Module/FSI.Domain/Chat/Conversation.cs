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
    public class Conversation : FullAuditedAggregateRoot<Guid>
    {
        public bool? JustTwoPeople { get; set; }

        public bool? IsActiveA { get; set; }

        public bool? IsActiveB { get; set; }

        public bool? IsStorageA { get; set; }

        public bool? IsStorageB { get; set; }

        public int? LastIndexSeenA { get; set; }

        public int? LastIndexSeenB { get; set; }

        public Guid? UserAId { get; set; }

        public UserRoot? UserA { get; set; }

        public Guid? UserBId { get; set; }

        public UserRoot? UserB { get; set; }

        public string? ConversationName { get; set; }

        public string? ConversationAvatar { get; set; }

        public string? Tag { get; set; }

        public string? JoinLink { get; set; }

        public Guid? LastMessageId { get; set; }

        [NotMapped]
        public Message? LastMessage { get; set; }

        public List<MeetInviteKey>? MeetInviteKeys { get; set; }
    }

    public class MeetInviteKey
    {
        public string InviteKey { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
