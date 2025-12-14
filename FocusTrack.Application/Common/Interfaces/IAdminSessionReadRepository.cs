using FocusTrack.Application.Admin.Queries.FilterSessions;
using FocusTrack.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Common.Interfaces
{
    public interface IAdminSessionReadRepository
    {
        Task<PagedResult<Admin.Queries.FilterSessions.AdminSessionRowDto>> FilterAsync(
            FilterSessionsQuery query,
            CancellationToken ct = default);
    }
}
