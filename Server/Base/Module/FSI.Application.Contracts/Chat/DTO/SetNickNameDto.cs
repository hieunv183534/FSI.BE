using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class SetNickNameDto
    {
        public Guid ConversationId { get; set; }

        public Guid UserId { get; set; }

        public string NickName { get; set; }
    }
}
