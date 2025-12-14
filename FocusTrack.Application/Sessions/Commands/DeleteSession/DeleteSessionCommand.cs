using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.DeleteSession
{

    public sealed record DeleteSessionCommand(Guid Id) : IRequest;
}
