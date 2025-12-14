using FocusTrack.NotificationWorker.Models;
using FocusTrack.NotificationWorker.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Services
{
    public class NotificationProcessor
    {
        private readonly NotificationDbContext _db;
        private readonly UserPresenceTracker _presence;
        private readonly RealTimeNotifier _rt;
        private readonly EmailNotifier _email;

        public NotificationProcessor(
            NotificationDbContext db,
            UserPresenceTracker presence,
            RealTimeNotifier rt,
            EmailNotifier email)
        {
            _db = db;
            _presence = presence;
            _rt = rt;
            _email = email;
        }

        public async Task ProcessAsync(string routingKey, string json)
        {
            if (routingKey.EndsWith("SessionSharedDomainEvent"))
            {
                var evt = JsonSerializer.Deserialize<SessionSharedIntegrationEvent>(json)!;
                await HandleSessionShared(evt);
            }
            else if (routingKey.EndsWith("SessionUnsharedDomainEvent"))
            {
                var evt = JsonSerializer.Deserialize<SessionUnsharedIntegrationEvent>(json)!;
                await HandleSessionUnshared(evt);
            }
            else if (routingKey.EndsWith("DailyGoalAchievedIntegrationEvent"))
            {
                var evt = JsonSerializer.Deserialize<DailyGoalAchievedIntegrationEvent>(json)!;
                await HandleDailyGoalAchieved(evt);
            }
        }

        private async Task HandleSessionShared(SessionSharedIntegrationEvent evt)
        {
            var msg = $"A session has been shared with you by user {evt.OwnerUserId}.";

            await StoreNotification(evt.RecipientUserId, msg);

            if (_presence.IsOnline(evt.RecipientUserId))
                await _rt.PushToUserAsync(evt.RecipientUserId, msg);
            else
                await _email.SendEmailAsync("user@example.com", "Session Shared", msg);
        }

        private async Task HandleSessionUnshared(SessionUnsharedIntegrationEvent evt)
        {
            var msg = $"A session previously shared with you has been revoked by user {evt.OwnerUserId}.";

            await StoreNotification(evt.RecipientUserId, msg);

            if (_presence.IsOnline(evt.RecipientUserId))
                await _rt.PushToUserAsync(evt.RecipientUserId, msg);
            else
                await _email.SendEmailAsync("user@example.com", "Session Unshared", msg);
        }

        private async Task HandleDailyGoalAchieved(DailyGoalAchievedIntegrationEvent evt)
        {
            var msg = $"Congratulations! You achieved your daily focus goal on {evt.Date:D}.";

            await StoreNotification(evt.UserId, msg);

            if (_presence.IsOnline(evt.UserId))
                await _rt.PushToUserAsync(evt.UserId, msg);
            else
                await _email.SendEmailAsync("user@example.com", "Daily Goal Achieved", msg);
        }

        private async Task StoreNotification(Guid userId, string message)
        {
            _db.Notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Message = message,
                CreatedAt = DateTimeOffset.UtcNow,
                IsRead = false
            });

            await _db.SaveChangesAsync();
        }
    }
}
