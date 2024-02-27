using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class CreateMeetInviteKeyDto
    {
        public Guid ConversationId { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
