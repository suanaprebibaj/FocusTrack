using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Services
{
    public class UserPresenceTracker
    {
        private readonly HashSet<Guid> _onlineUsers = [];

        public void MarkOnline(Guid userId) => _onlineUsers.Add(userId);
        public void MarkOffline(Guid userId) => _onlineUsers.Remove(userId);
        public bool IsOnline(Guid userId) => _onlineUsers.Contains(userId);
    }
}
