using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.RewardWorker.Services
{
    public class RewardCalculator
    {
        private const decimal Threshold = 120.0m;

        public bool ShouldTriggerBadge(decimal previousTotal, decimal newTotal)
            => previousTotal < Threshold && newTotal >= Threshold;
    }
}
