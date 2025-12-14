using FocusTrack.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Sessions
{
    public sealed class SessionShare : Entity<Guid>
    {
        public Guid SessionId { get; private set; }
        public Guid RecipientUserId { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }

        private SessionShare() : base(Guid.Empty)
        {
            // For EF
        }

        private SessionShare(Guid id, Guid sessionId, Guid recipientUserId)
            : base(id)
        {
            SessionId = sessionId;
            RecipientUserId = recipientUserId;
            CreatedAt = DateTimeOffset.UtcNow;
            IsRevoked = false;
        }

        public static SessionShare Create(Guid id, Guid sessionId, Guid recipientUserId)
            => new(id, sessionId, recipientUserId);

        public void Revoke()
        {
            if (IsRevoked) return;

            IsRevoked = true;
            RevokedAt = DateTimeOffset.UtcNow;
        }
    }
}
