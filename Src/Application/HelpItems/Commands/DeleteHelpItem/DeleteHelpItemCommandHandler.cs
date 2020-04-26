//using ColabSpace.Application.Common.Exceptions;
//using ColabSpace.Application.Common.Interfaces;
//using ColabSpace.Domain.Entities;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ColabSpace.Application.HelpItems.Commands.DeleteHelpItem
//{
//    public class DeleteHelpItemCommandHandler : IRequestHandler<DeleteHelpItemCommand>
//    {
//        private readonly IColabSpaceDbContext _context;

//        public DeleteHelpItemCommandHandler(IColabSpaceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Unit> Handle(DeleteHelpItemCommand request, CancellationToken cancellationToken)
//        {
//            var helpItem = await _context.HelpItems.FindAsync(request.Id);

//            if (helpItem == null)
//            {
//                throw new NotFoundException(nameof(HelpItem), request.Id);
//            }

//            _context.HelpItems.Remove(helpItem);

//            await _context.SaveChangesAsync(cancellationToken);

//            return Unit.Value;
//        }
//    }
//}
