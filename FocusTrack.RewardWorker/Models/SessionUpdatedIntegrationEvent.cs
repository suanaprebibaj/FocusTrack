using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.RewardWorker.Models
{
    public class SessionUpdatedIntegrationEvent
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public decimal DurationMinutes { get; set; }
    }
}
