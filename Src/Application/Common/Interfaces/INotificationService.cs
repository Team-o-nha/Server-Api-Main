using ColabSpace.Application.Notifications.Models;
using System.Threading.Tasks;

namespace ColabSpace.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(MessageDto message);
    }
}
