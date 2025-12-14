using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Queries.GetSessionById
{
    public sealed record GetSessionByIdQuery(Guid Id) : IRequest<SessionDto>;

    public sealed record SessionDto(
        Guid Id,
        string Topic,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime,
        string Mode,
        decimal DurationMinutes,
        bool IsDailyGoalAchieved);
}
