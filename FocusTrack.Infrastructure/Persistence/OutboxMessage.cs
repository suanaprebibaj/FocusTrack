using FocusTrack.Domain.Primitives;
using FocusTrack.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Persistence
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public DateTimeOffset OccurredOnUtc { get; private set; }
        public string Type { get; private set; } = default!;
        public string Content { get; private set; } = default!;
        public DateTimeOffset? ProcessedOnUtc { get; private set; }
        public string? Error { get; private set; }

        private OutboxMessage() { }

        private OutboxMessage(Guid id, DateTimeOffset occurredOnUtc, string type, string content)
        {
            Id = id;
            OccurredOnUtc = occurredOnUtc;
            Type = type;
            Content = content;
        }

        public static OutboxMessage FromDomainEvent(IDomainEvent @event, IDomainEventSerializer serializer)
        {
            var content = serializer.Serialize(@event);
            return new OutboxMessage(@event.Id, @event.OccurredOn, @event.GetType().FullName!, content);
        }

        public void MarkProcessed() => ProcessedOnUtc = DateTimeOffset.UtcNow;

        public void MarkFailed(string error)
        {
            Error = error;
            ProcessedOnUtc = DateTimeOffset.UtcNow;
        }
    }
}
