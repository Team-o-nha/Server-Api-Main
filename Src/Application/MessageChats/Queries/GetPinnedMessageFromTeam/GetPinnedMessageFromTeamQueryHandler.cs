using AutoMapper;
using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColabSpace.Application.MessageChats.Queries.GetPinnedMessageFromTeam
{
    public class GetPinnedMessageFromTeamQueryHandler : IRequestHandler<GetPinnedMessageFromTeamQuery, IEnumerable<PinMessageDto>>
    {
        private readonly static string databaseName = "ColabSpaceDb";
        private readonly static string containerName = "MessageChats";
        private readonly int pageSize = 30;

        private readonly IColabSpaceDbContext _context;
        private readonly IMapper _mapper;

        public GetPinnedMessageFromTeamQueryHandler(IColabSpaceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PinMessageDto>> Handle(GetPinnedMessageFromTeamQuery request, CancellationToken cancellationToken)
        {
            // kiem tra neu khong truyen PageIndex thi lay trang dau
            request.PageIndex ??= 1;

            // lay danh sach public channel
            var listChannelIds = await _context.Conversations
                .Where(channel => channel.TeamId == request.TeamId.ToString() && channel.isPublic == true)
                // them dau ' truoc va sau id
                .Select(channel => $"'{channel.Id}'")
                .ToListAsync();

            // lay danh sach message thuoc public channel cua team
            var lstMessageChatsQuery = $" SELECT * FROM parent " +
                $" WHERE parent.ConversationId IN({string.Join(',', listChannelIds)}) " +
                    $" AND (parent.IsPin = true" +
                        $" OR EXISTS(SELECT VALUE child FROM child IN parent.AttachFileList WHERE child.IsPin = true)) " +
                // vi khi pin file se cap nhat lai Pinned cua Message chua file nen khi sort theo thu tu thi
                // ca file va message chua file deu se len dau
                $" ORDER BY parent.PinnedDate DESC " +
                // phan trang
                $" OFFSET {((int)request.PageIndex - 1) * pageSize} LIMIT {pageSize} ";
            var lstMessageChats = await GetMessageChatsAsync(lstMessageChatsQuery);

            var lstPinnedItems = new List<PinMessageDto>();
            // lap tung message chat lay duoc
            foreach (var msg in lstMessageChats)
            {
                // neu msg chat duoc pin thi them vao danh sach
                if (msg.IsPin)
                {
                    lstPinnedItems.Add(new PinMessageDto
                    {
                        PinnedMessage = _mapper.Map<MessageChatModel>(msg),
                        PinnedDiscriminator = "PinnedMessage"
                    });
                }

                var msgWithoutAttachFile = _mapper.Map<MessageChatModel>(msg);
                msgWithoutAttachFile.AttachFileList = null;

                // neu attach file cua message duoc pin thi them vao danh sach
                var lstPinFiles = msg.AttachFileList?
                    .Where(file => file.IsPin == true)
                    .Select(file => new PinMessageDto
                    {
                        PinnedMessage = msgWithoutAttachFile,
                        PinnedFile = new FileDto
                        {
                            BlobStorageUrl = file.BlobStorageUrl,
                            FileName = file.FileName,
                            FileSize = file.FileSize,
                            FileStorageName = file.FileStorageName,
                            IsPin = file.IsPin,
                            ThumbnailImage = file.ThumbnailImage,
                            ConversationId = msg.ConversationId,
                            MessageId = msg.Id
                        },
                        PinnedDiscriminator = "PinnedFile"
                    });

                if (lstPinFiles != null && lstPinFiles.Any())
                {
                    lstPinnedItems.AddRange(lstPinFiles);
                }
            }

            return lstPinnedItems;
        }

        private async Task<List<MessageChat>> GetMessageChatsAsync(string queryStr)
        {
            var cosmosClient = _context.GetCosmosClient;
            var container = cosmosClient.GetContainer(databaseName, containerName);

            var query = container.GetItemQueryIterator<MessageChat>(new QueryDefinition(queryStr));
            var results = new List<MessageChat>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

    }
}
