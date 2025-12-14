using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Common.Interfaces
{
    public interface IMessageBus
    {
        Task PublishAsync(string type, string payload, CancellationToken ct = default);
    }
}
