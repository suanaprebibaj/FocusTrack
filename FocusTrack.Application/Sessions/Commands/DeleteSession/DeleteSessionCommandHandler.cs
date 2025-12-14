using FocusTrack.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.DeleteSession
{

    public sealed class DeleteSessionCommandHandler
        : IRequestHandler<DeleteSessionCommand>
    {
        private readonly ISessionRepository _sessions;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;

        public DeleteSessionCommandHandler(
            ISessionRepository sessions,
            IUnitOfWork uow,
            ICurrentUserService currentUser)
        {
            _sessions = sessions;
            _uow = uow;
            _currentUser = currentUser;
        }

        public async Task Handle(DeleteSessionCommand request, CancellationToken ct)
        {
            var userId = _currentUser.GetUserId();
            var session = await _sessions.GetByIdAsync(request.Id, ct);

            if (session is null || session.UserId != userId)
                throw new UnauthorizedAccessException("You cannot delete this session.");

            session.Delete(); 
            _sessions.Remove(session);

            await _uow.SaveChangesAsync(ct);
            
        }
    }

}
