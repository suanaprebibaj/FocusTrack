using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Common.Interfaces
{
    public interface IPublicLinkService
    {
        Task<string> CreatePublicLinkAsync(Guid sessionId, CancellationToken ct = default);
    }
}
