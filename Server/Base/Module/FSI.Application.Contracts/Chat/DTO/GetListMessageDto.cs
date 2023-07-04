using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Chat.DTO
{
    public class GetListMessageDto : PagedAndSortedResultRequestDto
    {
        public Guid ConversationId { get; set; }
    }
}
