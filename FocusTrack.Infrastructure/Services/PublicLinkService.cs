using FocusTrack.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Services
{
    public class PublicLinkService : IPublicLinkService
    {
        private readonly IConfiguration _config;

        public PublicLinkService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> CreatePublicLinkAsync(Guid sessionId, CancellationToken ct = default)
        {
            var baseUrl = _config["PublicLinks:BaseUrl"] ?? "https://focustrack.local/share";

         
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(sessionId.ToString()));
            var token = Convert.ToHexString(bytes)[..16].ToLowerInvariant();

            var url = $"{baseUrl}/{token}";
            

            return Task.FromResult(url);
        }
    }
}
