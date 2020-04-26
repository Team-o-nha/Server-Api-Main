using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Notifications.Command
{
    public class UnReadNotificationCommandHandler : IRequestHandler<UnReadNotificationCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;


        public UnReadNotificationCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UnReadNotificationCommand request, CancellationToken cancellationToken)
        {
            Notification notification = await _context.Notifications.FindAsync(request.NotificationId);

            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), request.NotificationId);
            }

            notification.isRead = false;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
