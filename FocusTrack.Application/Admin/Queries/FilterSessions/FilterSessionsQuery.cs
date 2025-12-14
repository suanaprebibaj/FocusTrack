using FocusTrack.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Admin.Queries.FilterSessions
{
    public sealed record FilterSessionsQuery(
     Guid? UserId,
     string? Mode,
     DateTimeOffset? StartDateFrom,
     DateTimeOffset? StartDateTo,
     int Page = 1,
     int PageSize = 50) : IRequest<PagedResult<AdminSessionRowDto>>;

    public sealed record AdminSessionRowDto(
        Guid Id,
        Guid UserId,
        string Topic,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime,
        string Mode,
        decimal DurationMinutes,
        bool IsDailyGoalAchieved);
}
