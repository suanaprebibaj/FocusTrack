using FocusTrack.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FocusTrack.Application.Common.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Session session, CancellationToken ct = default);
        void Remove(Session session);

        Task<IReadOnlyList<Session>> GetUserSessionsPagedAsync(
            Guid userId,
            int page,
            int pageSize,
            CancellationToken ct = default);

        Task<int> CountUserSessionsAsync(Guid userId, CancellationToken ct = default);
    }
}
