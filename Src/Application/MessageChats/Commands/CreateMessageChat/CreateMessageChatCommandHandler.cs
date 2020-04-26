using ColabSpace.Application.Common.Exceptions;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Commands.CreateMessageChat
{
    public class CreateMessageChatCommandHandler : IRequestHandler<CreateMessageChatCommand, Guid>
    {
        private readonly IColabSpaceDbContext _context;

        public CreateMessageChatCommandHandler(IColabSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateMessageChatCommand request, CancellationToken cancellationToken)
        {
            // check conversation exists
            var conversation = await _context.Conversations.FindAsync(request.ConversationId);
            if (conversation == null)
            {
                throw new NotFoundException(nameof(Conversation), request.ConversationId);
            }

            // check login user belong to member of conversation
            if (!conversation.Members.Any(user => user.UserOid.Equals(request.RegUserId)))
            {
                throw new NotOwnedException(nameof(Conversation), request.ConversationId);
            }


            List<AttachFile> attachFiles = new List<AttachFile>();
            if (request.AttachFileList != null)
            {
                foreach (AttachFileModel file in request.AttachFileList)
                {
                    attachFiles.Add(new AttachFile()
                    {
                        FileName = file.FileName,
                        FileStorageName = file.FileStorageName,
                        BlobStorageUrl = file.BlobStorageUrl,
                        FileSize = file.FileSize,
                        ThumbnailImage = file.ThumbnailImage
                    });
                }
            }

            // copy prop from request
            var messageChat = new MessageChat
            {
                Content = request.Content,
                ConversationId = request.ConversationId,
                ConversationName = conversation.Name,
                RegUserName = request.RegUserName,
                AttachFileList = attachFiles,
                CreatedBy = request.RegUserId,
                ReactionList = new List<Reaction>(),
                IsPin = false,
                PinnedDate = DateTime.UtcNow,
                RelatedMessagesId = request.RelatedMessagesId,
                RelatedTaskId = request.RelatedTaskId,
            };

            // register new conversation
            _context.MessageChats.Add(messageChat);
            // isHide false for all
            foreach (var member in conversation.Members)
            {
                member.isHidden = false;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return messageChat.Id;

        }
    }
}
