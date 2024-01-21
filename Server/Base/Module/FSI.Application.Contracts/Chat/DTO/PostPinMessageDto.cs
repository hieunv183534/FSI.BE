using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class PostPinMessageDto
    {
        public Guid MessageId { get; set; }

        public bool IsPin { get; set; }
    }
}
