using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Queries.GetSessionsPaged
{
    public sealed class GetSessionsPagedQueryHandler
     : IRequestHandler<GetSessionsPagedQuery, PagedResult<SessionListItemDto>>
    {
        private readonly ISessionRepository _sessions;
        private readonly ICurrentUserService _currentUser;

        public GetSessionsPagedQueryHandler(
            ISessionRepository sessions,
            ICurrentUserService currentUser)
        {
            _sessions = sessions;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<SessionListItemDto>> Handle(GetSessionsPagedQuery request, CancellationToken ct)
        {
            var userId = _currentUser.GetUserId();
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

            var sessions = await _sessions.GetUserSessionsPagedAsync(userId, page, pageSize, ct);
            var total = await _sessions.CountUserSessionsAsync(userId, ct);

            var items = sessions.Select(s => new SessionListItemDto(
                s.Id,
                s.Topic,
                s.StartTime,
                s.EndTime,
                s.Mode.ToString(),
                s.Duration.Value,
                s.IsDailyGoalAchieved))
                .ToList();

            return new PagedResult<SessionListItemDto>(items, total, page, pageSize);
        }
    }
}
