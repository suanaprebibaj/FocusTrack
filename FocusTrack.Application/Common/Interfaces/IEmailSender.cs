using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
    }
}
