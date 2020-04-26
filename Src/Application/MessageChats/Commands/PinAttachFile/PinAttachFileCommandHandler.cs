using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Commands.PinAttachFile
{
    public class PinAttachFileCommandHandler : IRequestHandler<PinAttachFileCommand>
    {
        private readonly IColabSpaceDbContext _context;

        public PinAttachFileCommandHandler(IColabSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PinAttachFileCommand request, CancellationToken cancellationToken)
        {
            // get Message
            var messageChat = await _context.MessageChats.FindAsync(request.MessageId);
            if (messageChat == null)
            {
                throw new NotFoundException(nameof(MessageChat), request.MessageId);
            }

            // get attachfile
            var attachFile = messageChat.AttachFileList?
                .Where(file => file.BlobStorageUrl == request.BlobStorageUrl)
                .FirstOrDefault();
            if (attachFile == null)
            {
                throw new NotFoundException(nameof(AttachFile), request.BlobStorageUrl);
            }

            // update attach file
            attachFile.IsPin = request.IsPinFile;
            // update lai pinned date cua message
            messageChat.PinnedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
