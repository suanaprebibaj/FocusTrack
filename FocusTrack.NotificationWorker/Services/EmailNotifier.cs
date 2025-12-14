using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Services
{
    public class EmailNotifier
    {
        public Task SendEmailAsync(string email, string subject, string body)
        {
            Console.WriteLine($"EMAIL → {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}
