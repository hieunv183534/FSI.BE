using FSI.Application.Contracts.User.DTO;
using FSI.Domain.Chat;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class ConversationDto : FullAuditedAggregateRoot<Guid>
    {
        public bool? JustTwoPeople { get; set; }

        public bool? IsActiveA { get; set; }

        public bool? IsActiveB { get; set; }

        public bool? IsStorageA { get; set; }

        public bool? IsStorageB { get; set; }

        public int? LastIndexSeenA { get; set; }

        public int? LastIndexSeenB { get; set; }

        public Guid? UserAId { get; set; }

        public UserRootDto? UserA { get; set; }

        public Guid? UserBId { get; set; }

        public UserRootDto? UserB { get; set; }

        public string? ConversationName { get; set; }

        public string? ConversationAvatar { get; set; }

        public string? Tag { get; set; }

        public string? JoinLink { get; set; }

        public Guid? LastMessageId { get; set; }

        public MessageDto? LastMessage { get; set; }

        public bool? IsSeen { get; set; }
    }
}
