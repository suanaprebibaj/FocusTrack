using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Domain.Sessions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FocusTrack.Application.Sessions.Commands.CreateSession
{
    public sealed class CreateSessionCommandHandler
     : IRequestHandler<CreateSessionCommand, Guid>
    {
        private readonly ISessionRepository _sessions;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;

        public CreateSessionCommandHandler(
            ISessionRepository sessions,
            IUnitOfWork uow,
            ICurrentUserService currentUser)
        {
            _sessions = sessions;
            _uow = uow;
            _currentUser = currentUser;
        }

        public async Task<Guid> Handle(CreateSessionCommand request, CancellationToken ct)
        {
            var userId = _currentUser.GetUserId();
            var mode = Enum.Parse<SessionMode>(request.Mode, ignoreCase: true);

            var session = Session.Create(
                userId,
                request.Topic,
                request.StartTime,
                request.EndTime,
                mode);

            await _sessions.AddAsync(session, ct);
            await _uow.SaveChangesAsync(ct);

            return session.Id;
        }
    }
}
