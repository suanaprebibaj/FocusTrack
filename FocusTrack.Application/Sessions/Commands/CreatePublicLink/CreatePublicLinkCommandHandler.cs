using FocusTrack.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Sessions.Commands.CreatePublicLink
{
    public sealed class CreatePublicLinkCommandHandler
     : IRequestHandler<CreatePublicLinkCommand, string>
    {
        private readonly ISessionRepository _sessions;
        private readonly ICurrentUserService _currentUser;
        private readonly IPublicLinkService _publicLinkService;

        public CreatePublicLinkCommandHandler(
            ISessionRepository sessions,
            ICurrentUserService currentUser,
            IPublicLinkService publicLinkService)
        {
            _sessions = sessions;
            _currentUser = currentUser;
            _publicLinkService = publicLinkService;
        }

        public async Task<string> Handle(CreatePublicLinkCommand request, CancellationToken ct)
        {
            var userId = _currentUser.GetUserId();
            var session = await _sessions.GetByIdAsync(request.SessionId, ct);

            if (session is null || session.UserId != userId)
                throw new UnauthorizedAccessException("You cannot share this session.");

            var url = await _publicLinkService.CreatePublicLinkAsync(session.Id, ct);
            return url;
        }
    }

}
