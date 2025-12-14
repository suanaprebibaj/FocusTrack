using FocusTrack.Application.Admin.Queries.FilterSessions;
using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Application.Common.Models;
using FocusTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Repositories
{
    public class AdminSessionReadRepository : IAdminSessionReadRepository
    {
        private readonly AppDbContext _db;

        public AdminSessionReadRepository(AppDbContext db) => _db = db;

        public async Task<PagedResult<AdminSessionRowDto>> FilterAsync(
            FilterSessionsQuery query,
            CancellationToken ct = default)
        {
            var q = _db.Sessions.AsQueryable();

            if (query.UserId.HasValue)
                q = q.Where(s => s.UserId == query.UserId.Value);

            if (!string.IsNullOrWhiteSpace(query.Mode))
                q = q.Where(s => s.Mode.ToString() == query.Mode);

            if (query.StartDateFrom.HasValue)
                q = q.Where(s => s.StartTime >= query.StartDateFrom.Value);

            if (query.StartDateTo.HasValue)
                q = q.Where(s => s.StartTime <= query.StartDateTo.Value);

            var total = await q.CountAsync(ct);

            var page = query.Page <= 0 ? 1 : query.Page;
            var pageSize = query.PageSize <= 0 ? 50 : query.PageSize;

            var items = await q
                .OrderByDescending(s => s.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new AdminSessionRowDto(
                    s.Id,
                    s.UserId,
                    s.Topic,
                    s.StartTime,
                    s.EndTime,
                    s.Mode.ToString(),
                    s.Duration.Value,
                    s.IsDailyGoalAchieved))
                .ToListAsync(ct);

            return new PagedResult<AdminSessionRowDto>(items, total, page, pageSize);
        }
    }
}
