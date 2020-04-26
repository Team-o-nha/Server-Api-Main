using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.Notifications.Command
{
    public class ReadNotificationCommandHandler : IRequestHandler<ReadNotificationCommand>
    {
        private readonly IColabSpaceDbContext _context;
        private readonly ICurrentUserService _currentUserService;


        public ReadNotificationCommandHandler(IColabSpaceDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            Notification notification = await _context.Notifications.FindAsync(request.NotificationId);

            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), request.NotificationId);
            }

            notification.isRead = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
