using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Domain.Users;
using FocusTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) => _db = db;

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.Id == id, ct);
        }

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            _db.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}
