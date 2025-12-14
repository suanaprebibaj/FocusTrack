using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Common.Models
{
    public sealed record PagedResult<T>(
       IReadOnlyList<T> Items,
       int TotalCount,
       int Page,
       int PageSize);
}
