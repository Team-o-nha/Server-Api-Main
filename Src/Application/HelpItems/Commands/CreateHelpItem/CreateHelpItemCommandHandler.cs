//using ColabSpace.Application.Common.Interfaces;
//using ColabSpace.Domain.Entities;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ColabSpace.Application.HelpItems.Commands.CreateHelpItem
//{
//    public class CreateHelpItemCommandHandler : IRequestHandler<CreateHelpItemCommand, Guid>
//    {
//        private readonly IColabSpaceDbContext _context;

//        public CreateHelpItemCommandHandler(IColabSpaceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Guid> Handle(CreateHelpItemCommand request, CancellationToken cancellationToken)
//        {
//            var helpItem = new HelpItem()
//            {
//                Name = request.Name,
//                Description = request.Description,
//                Content = new AttachFile()
//                {
//                    FileName = request.Content.FileName,
//                    FileSize = request.Content.FileSize,
//                    BlobStorageUrl = request.Content.BlobStorageUrl,
//                    FileStorageName = request.Content.FileStorageName,
//                }
//            };

//            _context.HelpItems.Add(helpItem);
//            await _context.SaveChangesAsync(cancellationToken);

//            return helpItem.Id;
//        }
//    }
//}
