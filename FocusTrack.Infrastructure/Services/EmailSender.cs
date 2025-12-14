using FocusTrack.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
        {
            // In a real system, pull these from configuration(secrets)
            var host = _config["Email:Smtp:Host"];
            var port = int.Parse(_config["Email:Smtp:Port"] ?? "25");
            var from = _config["Email:From"] ?? "no-reply@focustrack.local";

            if (string.IsNullOrWhiteSpace(host))
            {
                _logger.LogWarning("SMTP host not configured. Skipping email to {To}.", to);
                return;
            }

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = false,
                Credentials = CredentialCache.DefaultNetworkCredentials
            };

            using var message = new MailMessage(from, to, subject, body);

            await client.SendMailAsync(message, ct);
            _logger.LogInformation("Email sent to {To} with subject {Subject}.", to, subject);
        }
    }
}
