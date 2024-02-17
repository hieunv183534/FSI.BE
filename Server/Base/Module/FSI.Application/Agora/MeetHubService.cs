using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Agora
{
    public class MeetHubService
    {
        public List<UserMeet> UserMeets { get; set; } = new List<UserMeet>();

        public List<GuestMeet> GuestMeets { get; set; } = new List<GuestMeet>();
    }

    public class UserMeet
    {
        public Guid ConversationId { get; set; }

        public Guid Uid { get; set; }

        public string ConnectionId { get; set; }

        public bool Video { get; set; }

        public bool Micro { get; set; }

        public bool Screen { get; set; }
    }

    public class GuestMeet
    {
        public Guid GuestId { get; set; }

        public Guid ConversationId { get; set; }

        public string GuestName { get; set; }
    }
}
