using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Domain.Sessions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.UpdateSession
{
    public sealed class UpdateSessionCommandHandler
     : IRequestHandler<UpdateSessionCommand>
    {
        private readonly ISessionRepository _sessions;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;

        public UpdateSessionCommandHandler(
            ISessionRepository sessions,
            IUnitOfWork uow,
            ICurrentUserService currentUser)
        {
            _sessions = sessions;
            _uow = uow;
            _currentUser = currentUser;
        }

        public async Task Handle(UpdateSessionCommand request, CancellationToken ct)
        {
            var userId = _currentUser.GetUserId();
            var session = await _sessions.GetByIdAsync(request.Id, ct);

            if (session is null || session.UserId != userId)
                throw new UnauthorizedAccessException("You cannot modify this session.");

            var mode = Enum.Parse<SessionMode>(request.Mode, ignoreCase: true);

            session.Update(
                request.Topic,
                request.StartTime,
                request.EndTime,
                mode);

            await _uow.SaveChangesAsync(ct);

        }
    
}

}
