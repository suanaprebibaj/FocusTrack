using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Models
{
    public class SessionSharedIntegrationEvent
    {
        public Guid SessionId { get; set; }
        public Guid OwnerUserId { get; set; }
        public Guid RecipientUserId { get; set; }
    }
}
