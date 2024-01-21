using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class MessageSendToConversationDto
    {
        public MessageType Type { get; set; }

        public string Content { get; set; }

        public Guid? FocusToMessageId { get; set; }

        public Guid ConversationId { get; set; }
    }
}
