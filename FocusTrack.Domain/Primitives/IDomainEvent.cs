using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Domain.Primitives
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTimeOffset OccurredOn { get; }
    }
}
