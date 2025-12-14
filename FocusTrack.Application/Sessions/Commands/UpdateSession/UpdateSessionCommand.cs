using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.UpdateSession
{
    public sealed record UpdateSessionCommand(
     Guid Id,
     string Topic,
     DateTimeOffset StartTime,
     DateTimeOffset EndTime,
     string Mode) : IRequest;
}
