using FocusTrack.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Queries.GetSessionById
{

    public sealed class GetSessionByIdQueryHandler
        : IRequestHandler<GetSessionByIdQuery, SessionDto>
    {
        private readonly ISessionRepository _sessions;
        private readonly ICurrentUserService _currentUser;

        public GetSessionByIdQueryHandler(
            ISessionRepository sessions,
            ICurrentUserService currentUser)
        {
            _sessions = sessions;
            _currentUser = currentUser;
        }

        public async Task<SessionDto> Handle(GetSessionByIdQuery request, CancellationToken ct)
        {
            var userId = _currentUser.GetUserId();
            var session = await _sessions.GetByIdAsync(request.Id, ct);

            if (session is null || session.UserId != userId)
                throw new UnauthorizedAccessException("You cannot view this session.");

            return new SessionDto(
                session.Id,
                session.Topic,
                session.StartTime,
                session.EndTime,
                session.Mode.ToString(),
                session.Duration.Value,
                session.IsDailyGoalAchieved);
        }
    }
}
