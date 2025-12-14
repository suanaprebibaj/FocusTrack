using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Domain.Users;
using MediatR;


namespace FocusTrack.Application.Admin.Commands.ChangeUserStatus
{
    public sealed class ChangeUserStatusCommandHandler
     : IRequestHandler<ChangeUserStatusCommand>
    {
        private readonly IUserRepository _users;
        private readonly IUnitOfWork _uow;

        public ChangeUserStatusCommandHandler(IUserRepository users, IUnitOfWork uow)
        {
            _users = users;
            _uow = uow;
        }

        public async Task Handle(ChangeUserStatusCommand request, CancellationToken ct)
        {
            var user = await _users.GetByIdAsync(request.UserId, ct)
                ?? throw new KeyNotFoundException("User not found.");

            if (!Enum.TryParse<UserStatus>(request.NewStatus, true, out var status))
                throw new ArgumentException("Invalid user status.");

            user.ChangeStatus(status); 
            await _users.UpdateAsync(user, ct);

            await _uow.SaveChangesAsync(ct);
        }
    }
}
