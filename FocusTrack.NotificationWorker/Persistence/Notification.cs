using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Persistence
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
