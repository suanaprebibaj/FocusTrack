using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Admin.Commands.ChangeUserStatus
{
    public sealed record ChangeUserStatusCommand(
    Guid UserId,
    string NewStatus) : IRequest;
}
