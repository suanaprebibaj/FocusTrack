using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.RewardWorker.Models
{
    public class DailyGoalAchievedIntegrationEvent
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
