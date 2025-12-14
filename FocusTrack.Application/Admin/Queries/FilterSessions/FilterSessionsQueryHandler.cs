using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Admin.Queries.FilterSessions
{

    public sealed class FilterSessionsQueryHandler
        : IRequestHandler<FilterSessionsQuery, PagedResult<AdminSessionRowDto>>
    {
        private readonly IAdminSessionReadRepository _repo;

        public FilterSessionsQueryHandler(IAdminSessionReadRepository repo)
            => _repo = repo;

        public Task<PagedResult<AdminSessionRowDto>> Handle(FilterSessionsQuery request, CancellationToken ct)
            => _repo.FilterAsync(request, ct);
    }
}
