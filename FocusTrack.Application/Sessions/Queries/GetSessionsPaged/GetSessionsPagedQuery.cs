using FocusTrack.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Queries.GetSessionsPaged
{
    public sealed record GetSessionsPagedQuery(int Page, int PageSize)
     : IRequest<PagedResult<SessionListItemDto>>;

    public sealed record SessionListItemDto(
        Guid Id,
        string Topic,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime,
        string Mode,
        decimal DurationMinutes,
        bool IsDailyGoalAchieved);
}
