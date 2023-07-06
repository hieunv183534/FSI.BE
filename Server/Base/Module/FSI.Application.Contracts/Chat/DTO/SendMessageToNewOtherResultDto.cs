using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class SendMessageToNewOtherResultDto
    {
        public ConversationDto NewConversation { get; set; }

        public MessageDto NewMessage { get; set; }
    }
}
