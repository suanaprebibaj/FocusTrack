using FocusTrack.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Users.Events
{
    public sealed record UserStatusChangedDomainEvent(
     Guid UserId,
     UserStatus OldStatus,
     UserStatus NewStatus) : DomainEvent;
}
