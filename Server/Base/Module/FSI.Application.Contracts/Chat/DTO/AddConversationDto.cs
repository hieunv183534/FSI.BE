using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class AddConversationDto
    {
        public string ConversationName { get; set; }

        public List<Guid> MemberIds { get; set; }
    }
}
