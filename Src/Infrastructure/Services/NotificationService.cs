using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Notifications.Models;
using System.Threading.Tasks;

namespace ColabSpace.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendAsync(MessageDto message)
        {
            return Task.CompletedTask;
        }
    }
}
