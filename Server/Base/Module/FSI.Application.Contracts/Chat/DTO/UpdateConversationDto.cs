using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class UpdateConversationDto
    {
        public Guid ConversationId { get; set; }

        public string? AvatarUrl { get; set; }

        public string ConversationName { get; set; }
    }
}
