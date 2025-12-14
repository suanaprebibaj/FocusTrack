using FocusTrack.Domain.Primitives;
using FocusTrack.Domain.Sessions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Sessions
{
    public sealed class Session : AggregateRoot<Guid>
    {
        private readonly List<SessionShare> _shares = [];

        public Guid UserId { get; private set; }
        public string Topic { get; private set; } = default!;
        public DateTimeOffset StartTime { get; private set; }
        public DateTimeOffset EndTime { get; private set; }
        public SessionMode Mode { get; private set; }
        public DurationMinutes Duration { get; private set; } = null!;
        public bool IsDailyGoalAchieved { get; private set; }

        public IReadOnlyCollection<SessionShare> Shares => _shares.AsReadOnly();

        // EF
        private Session() : base(Guid.Empty)
        {
        }

        private Session(Guid id,
            Guid userId,
            string topic,
            DateTimeOffset start,
            DateTimeOffset end,
            SessionMode mode)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be empty.", nameof(topic));

            if (end <= start)
                throw new ArgumentException("EndTime must be after StartTime.");

            Id = id;
            UserId = userId;
            Topic = topic;
            StartTime = start;
            EndTime = end;
            Mode = mode;
            Duration = DurationMinutes.FromTimes(start, end);

            RaiseDomainEvent(new SessionCreatedDomainEvent(Id, UserId));
        }

        public static Session Create(Guid userId,
            string topic,
            DateTimeOffset start,
            DateTimeOffset end,
            SessionMode mode)
        {
            return new Session(Guid.NewGuid(), userId, topic, start, end, mode);
        }

        public void Update(
            string topic,
            DateTimeOffset start,
            DateTimeOffset end,
            SessionMode mode)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be empty.", nameof(topic));

            if (end <= start)
                throw new ArgumentException("EndTime must be after StartTime.");

            Topic = topic;
            StartTime = start;
            EndTime = end;
            Mode = mode;
            Duration = DurationMinutes.FromTimes(start, end);

            RaiseDomainEvent(new SessionUpdatedDomainEvent(Id, UserId));
        }

        public void MarkDailyGoalAchieved()
        {
            if (IsDailyGoalAchieved)
                return;

            IsDailyGoalAchieved = true;

            RaiseDomainEvent(new DailyGoalAchievedDomainEvent(Id, UserId, StartTime.Date));
        }

        public void Delete()
        {
            RaiseDomainEvent(new SessionDeletedDomainEvent(Id, UserId));
        }

        public void ShareWithUser(Guid recipientUserId)
        {
            if (_shares.Any(s => s.RecipientUserId == recipientUserId && !s.IsRevoked))
                return;

            var share = SessionShare.Create(Guid.NewGuid(), Id, recipientUserId);
            _shares.Add(share);

            RaiseDomainEvent(new SessionSharedDomainEvent(Id, UserId, recipientUserId));
        }

        public void RevokeShare(Guid recipientUserId)
        {
            var share = _shares.SingleOrDefault(s => s.RecipientUserId == recipientUserId && !s.IsRevoked);
            if (share is null)
                return;

            share.Revoke();
            RaiseDomainEvent(new SessionUnsharedDomainEvent(Id, UserId, recipientUserId));
        }
    }
}
