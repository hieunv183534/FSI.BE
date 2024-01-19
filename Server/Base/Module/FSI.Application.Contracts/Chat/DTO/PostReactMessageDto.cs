using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class PostReactMessageDto
    {
        public Guid MessageId { get; set; }

        public MessageReact? React { get; set; }
    }
}
