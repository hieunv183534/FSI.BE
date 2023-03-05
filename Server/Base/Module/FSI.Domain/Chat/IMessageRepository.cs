using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FSI.Domain.Chat
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {
    }
}
