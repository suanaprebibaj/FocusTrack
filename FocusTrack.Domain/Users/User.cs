using FocusTrack.Domain.Primitives;
using FocusTrack.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Users
{
    public sealed class User : AggregateRoot<Guid>
    {
        public string Email { get; private set; } = default!;
        public string? DisplayName { get; private set; }
        public UserStatus Status { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? StatusChangedAt { get; private set; }

        // EF
        private User() : base(Guid.Empty)
        {
        }

        private User(Guid id, string email, string? displayName)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Id = id;
            Email = email;
            DisplayName = displayName;
            Status = UserStatus.Active;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static User Create(Guid id, string email, string? displayName = null)
            => new(id, email, displayName);

        public void ChangeStatus(UserStatus newStatus)
        {
            if (Status == newStatus)
                return;

            var oldStatus = Status;
            Status = newStatus;
            StatusChangedAt = DateTimeOffset.UtcNow;

            RaiseDomainEvent(new UserStatusChangedDomainEvent(Id, oldStatus, newStatus));
        }

        public void ChangeDisplayName(string? displayName)
        {
            DisplayName = displayName;
        }
    }
}
