using FocusTrack.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        //public Guid GetUserId()
        //{
        //    var user = _httpContextAccessor.HttpContext?.User;

        //    var idStr = user?.FindFirstValue(ClaimTypes.NameIdentifier)
        //              ?? user?.FindFirstValue("sub");

        //    if (idStr is null)
        //        throw new InvalidOperationException("No user id in claims.");

        //    if (Guid.TryParse(idStr, out var id))
        //        return id;

        //    // If your Id is not Guid, you can map it, but for this task we assume Guid.
        //    throw new InvalidOperationException("User id is not a valid Guid.");
        //}
        public Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

           
            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                
                return Guid.Parse("11111111-1111-1111-1111-111111111111");
            }

            return Guid.Parse(userIdClaim);
        }
        public string? GetUserEmail()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.FindFirstValue(ClaimTypes.Email);
        }
    }
}
