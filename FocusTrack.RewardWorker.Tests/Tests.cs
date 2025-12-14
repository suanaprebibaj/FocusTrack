using FocusTrack.RewardWorker.Services;
using Xunit;

namespace FocusTrack.RewardWorker.Tests
{
    public class Tests
    {
        private readonly RewardCalculator _calculator = new();

        [Fact]
        public void Does_NOT_trigger_badge_when_total_is_below_120()
        {
          
            decimal previousTotal = 100.00m;
            decimal newTotal = 119.99m;
            var result = _calculator.ShouldTriggerBadge(previousTotal, newTotal);
            Assert.False(result);
        }

        [Fact]
        public void Triggers_badge_when_crossing_threshold_at_120()
        {
            
            decimal previousTotal = 119.99m;
            decimal newTotal = 120.00m;
            var result = _calculator.ShouldTriggerBadge(previousTotal, newTotal);
            Assert.True(result);
        }

        [Fact]
        public void Triggers_badge_when_crossing_threshold_above_120()
        {
            
            decimal previousTotal = 119.99m;
            decimal newTotal = 120.01m;
            var result = _calculator.ShouldTriggerBadge(previousTotal, newTotal);
            Assert.True(result);
        }

        [Fact]
        public void Does_NOT_trigger_badge_if_threshold_was_already_reached()
        {
            
            decimal previousTotal = 120.00m;
            decimal newTotal = 130.00m;
            var result = _calculator.ShouldTriggerBadge(previousTotal, newTotal);
            Assert.False(result);
        }
    }
}
