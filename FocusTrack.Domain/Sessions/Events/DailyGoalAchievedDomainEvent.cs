using FocusTrack.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Sessions.Events
{
    public sealed record DailyGoalAchievedDomainEvent(
     Guid SessionId,
     Guid UserId,
     DateTime Date) : DomainEvent;
}
