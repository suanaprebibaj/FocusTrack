using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Primitives
{
    public abstract record DomainEvent(Guid Id, DateTimeOffset OccurredOn) : IDomainEvent
    {
        protected DomainEvent() : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
        }
    }
}
