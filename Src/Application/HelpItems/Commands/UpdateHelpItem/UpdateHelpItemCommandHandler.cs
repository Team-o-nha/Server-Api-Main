//using ColabSpace.Application.Common.Exceptions;
//using ColabSpace.Application.Common.Interfaces;
//using ColabSpace.Domain.Entities;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ColabSpace.Application.HelpItems.Commands.UpdateHelpItem
//{
//    public class UpdateHelpItemCommandHandler : IRequestHandler<UpdateHelpItemCommand, Guid>
//    {
//        private readonly IColabSpaceDbContext _context;

//        public UpdateHelpItemCommandHandler(IColabSpaceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Guid> Handle(UpdateHelpItemCommand request, CancellationToken cancellationToken)
//        {
//            var helpItem = await _context.HelpItems.FindAsync(request.Id);

//            if (helpItem == null)
//            {
//                throw new NotFoundException(nameof(HelpItem), request.Id);
//            }

//            helpItem.Name = request.Name;
//            helpItem.Description = request.Description;
//            helpItem.Content = new AttachFile()
//            {
//                FileName = request.Content.FileName,
//                FileSize = request.Content.FileSize,
//                BlobStorageUrl = request.Content.BlobStorageUrl,
//                FileStorageName = request.Content.FileStorageName,
//            };

//            await _context.SaveChangesAsync(cancellationToken);

//            return helpItem.Id;
//        }
//    }
//}
