using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Domain.Sessions;
using FocusTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly AppDbContext _db;

        public SessionRepository(AppDbContext db) => _db = db;

        public async Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Sessions
                .Include("_shares")
                .SingleOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task AddAsync(Session session, CancellationToken ct = default)
            => await _db.Sessions.AddAsync(session, ct);

        public void Remove(Session session)
            => _db.Sessions.Remove(session);

        public async Task<IReadOnlyList<Session>> GetUserSessionsPagedAsync(
            Guid userId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            return await _db.Sessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public Task<int> CountUserSessionsAsync(Guid userId, CancellationToken ct = default)
        {
            return _db.Sessions.CountAsync(s => s.UserId == userId, ct);
        }
    }
}
