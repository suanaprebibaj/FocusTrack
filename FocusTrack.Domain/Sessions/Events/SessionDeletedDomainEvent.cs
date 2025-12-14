using FocusTrack.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Sessions.Events
{
    public sealed record SessionDeletedDomainEvent(Guid SessionId, Guid UserId)
     : DomainEvent;
}
