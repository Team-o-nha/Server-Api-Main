﻿using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Conversations.Queries.GetConversation;
using ColabSpace.Application.MessageChats.Commands.CreateMessageChat;
using ColabSpace.Application.MessageChats.Commands.PinAttachFile;
using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatById;
using ColabSpace.Application.Notifications.Command.CreateNotification;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.WebAPI.Hubs;
using ColabSpace.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class MessagesController : ApiController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _config;

        public MessagesController(IHubContext<ChatHub> hubContext,
            ICurrentUserService currentUserService,
            IBlobStorageService blobStorageService,
            IConfiguration config)
        {
            _hubContext = hubContext;
            _currentUserService = currentUserService;
            _blobStorageService = blobStorageService;
            _config = config;
        }

        [HttpPost("SendMessageToUser/{userId}")]
        public async Task<IActionResult> SendMessageToUser([FromRoute]string userId, [FromBody] MessageDto msg)
        {
            try
            {
                msg.UserId = _currentUserService.UserId;
                msg.Name = _currentUserService.UserName;
                if (msg.AttachFiles != null)
                {
                    foreach (AttachFileModel file in msg.AttachFiles)
                    {
                        file.BlobStorageUrl = await _blobStorageService
                            .UploadFileToBlobAsync(file.LocalUrl.Replace("\\\\", "\\"), file.FileStorageName);
                    }
                }

                Guid messageId = await Mediator.Send(new CreateMessageChatCommand()
                {
                    RegUserId = msg.UserId,
                    RegUserName = msg.Name,
                    Content = msg.Message,
                    ConversationId = msg.ConversationId,
                    AttachFileList = msg.AttachFiles,
                    RelatedMessagesId = msg.RelatedMessagesId,
                    RelatedTaskId = msg.RelatedTaskId
                });
                // edit message id back to MessageDto
                msg.MessageId = messageId.ToString();

                // query other members of the conversation
                var conversation = await Mediator.Send(new GetConversationQuery
                {
                    ConversationId = msg.ConversationId.ToString()
                });
                msg.TeamId = conversation.TeamId;

                // create Notification
                await CreateNotification(msg, messageId, "Mention", msg.Mentions);

                // Sent messages to Receiver
                msg.Type = "received";
                foreach (var entity in GetConnectionEntitys(userId))
                {
                    await _hubContext.Clients.Client(entity.RowKey).SendAsync("NewConversation", await GetLastConversationAsync(msg));
                    await _hubContext.Clients.Client(entity.RowKey).SendAsync("ReceiveMessage", msg);
                }

                // Sent messages to Sender
                msg.Type = "sent";
                foreach (var entity in GetConnectionEntitys(_currentUserService.UserId))
                {
                    await _hubContext.Clients.Client(entity.RowKey).SendAsync("ReceiveMessage", msg);
                }
            }
            catch (Exception)
            {
                if (msg.AttachFiles != null)
                {
                    foreach (AttachFileModel file in msg.AttachFiles)
                    {
                        await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                    }
                }
            }

            return NoContent();
        }

        [HttpPost("SendMessageToGroup/{conversationId}")]
        public async Task<IActionResult> SendMessageToGroup([FromRoute]string conversationId, [FromBody] MessageDto msg)
        {
            try
            {
                msg.UserId = _currentUserService.UserId;
                msg.Name = _currentUserService.UserName;
                if (msg.AttachFiles != null)
                {
                    foreach (AttachFileModel file in msg.AttachFiles)
                    {
                        file.BlobStorageUrl = await _blobStorageService
                            .UploadFileToBlobAsync(file.LocalUrl.Replace("\\\\", "\\"), file.FileStorageName);
                    }
                }

                Guid messageId = await Mediator.Send(new CreateMessageChatCommand()
                {
                    RegUserId = msg.UserId,
                    RegUserName = msg.Name,
                    Content = msg.Message,
                    ConversationId = msg.ConversationId,
                    AttachFileList = msg.AttachFiles,
                    RelatedMessagesId = msg.RelatedMessagesId,
                    RelatedTaskId = msg.RelatedTaskId,
                });
                // edit message id back to MessageDto
                msg.MessageId = messageId.ToString();

                // query other members of the conversation
                var conversation = await Mediator.Send(new GetConversationQuery
                {
                    ConversationId = msg.ConversationId.ToString()
                });
                msg.TeamId = conversation.TeamId;

                // create Notification
                await CreateNotification(msg, messageId, "Mention", msg.Mentions);

                // Sent messages to Receiver
                msg.Type = "received";
                var lastConversation = await GetLastConversationAsync(msg);
                // Add user to group
                foreach (UserModel user in lastConversation.Conversation.Members)
                {
                    foreach (var entity in GetConnectionEntitys(user.UserId.ToString()))
                    {
                        await _hubContext.Groups.AddToGroupAsync(entity.RowKey, conversationId);
                    }
                }
                var Connections = GetConnectionIds(_currentUserService.UserId);
                await _hubContext.Clients.GroupExcept(conversationId, Connections).SendAsync("NewConversation", lastConversation);
                await _hubContext.Clients.GroupExcept(conversationId, Connections).SendAsync("ReceiveMessage", msg);

                // Sent messages to Sender
                msg.Type = "sent";
                foreach (var entity in GetConnectionEntitys(_currentUserService.UserId))
                {
                    await _hubContext.Clients.Client(entity.RowKey).SendAsync("ReceiveMessage", msg);
                }
            }
            catch (Exception)
            {
                if (msg.AttachFiles != null)
                {
                    foreach (AttachFileModel file in msg.AttachFiles)
                    {
                        await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                    }
                }
            }

            return NoContent();
        }

        [HttpPut("{messageId}")]
        public async Task<ActionResult> Update(string messageId, UpdateMessageChatCommand command)
        {
            try
            {
                if (!messageId.Equals(command.MessageId.ToString()))
                {
                    return BadRequest();
                }

                var updateType = await Mediator.Send(command);

                // query message chat
                var messageChat = await Mediator.Send(new GetMessageChatByIdQuery
                {
                    MessageChatId = new Guid(messageId)
                });

                // query other members of the conversation
                var conversation = await Mediator.Send(new GetConversationQuery
                {
                    ConversationId = messageChat.ConversationId.ToString()
                });

                // create Notification
                if (messageChat.ReactionList.Count > 0
                    && updateType == "addReaction"
                    && messageChat.CreatedBy != _currentUserService.UserId)
                {
                    await CreateNotification(new MessageDto
                    {
                        UserId = _currentUserService.UserId,
                        Name = _currentUserService.UserName,
                        Message = messageChat.Content,
                        MessageId = messageChat.Id.ToString(),
                        ConversationId = messageChat.ConversationId,
                        Reactions = messageChat.ReactionList,
                        IsPin = messageChat.IsPin,
                        TeamId = conversation.TeamId
                    }, messageChat.Id, "Reaction",
                    conversation.Members.Where(u => u.UserId.ToString() == messageChat.CreatedBy).ToList());
                }

                // notify to other members of conversation about the new update message chat
                // Add user to group
                foreach (UserModel user in conversation.Members)
                {
                    foreach (var entity in GetConnectionEntitys(user.UserId.ToString()))
                    {
                        await _hubContext.Groups.AddToGroupAsync(entity.RowKey, conversation.Id.ToString());
                    }
                }
                var Connections = GetConnectionIds(_currentUserService.UserId);
                var msg = new MessageDto
                {
                    MessageId = messageChat.Id.ToString(),
                    ConversationId = messageChat.ConversationId,
                    Reactions = messageChat.ReactionList,
                    IsPin = messageChat.IsPin,
                    AttachFiles = messageChat.AttachFileList,
                    Date = messageChat.Created,
                    Message = messageChat.Content,
                    Name = messageChat.RegUserName,
                    ConversationName = conversation.Name

                };
                await _hubContext.Clients.GroupExcept(conversation.Id.ToString(), Connections).SendAsync("ReceiveMessage", msg);

                await _hubContext.Clients.GroupExcept(conversation.Id.ToString(), Connections).SendAsync("NewConversation", new ConversationLastContentModel
                {
                    Conversation = conversation,
                    LastMessageChatContent = new MessageChatModel
                    {
                        RegUserName = messageChat.RegUserName,
                        Content = messageChat.Content,
                        ConversationId = conversation.Id,
                        Created = messageChat.Created
                    }
                });

                foreach (var entity in GetConnectionEntitys(_currentUserService.UserId))
                {
                    await _hubContext.Clients.Client(entity.RowKey).SendAsync("ReceiveMessage", msg);
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"1111111111111111 :{e.Message} : {e.StackTrace}");
                return BadRequest();
            }
        }

        [HttpPut("pin-attached-file/{messageId}")]
        public async Task<ActionResult> PinAttachedFile(Guid messageId, PinAttachFileCommand command)
        {
            if (messageId != command.MessageId)
            {
                return BadRequest();
            }

            // cap nhat lai message chat
            await Mediator.Send(command);

            // query message chat
            var messageChat = await Mediator.Send(new GetMessageChatByIdQuery
            {
                MessageChatId = command.MessageId
            });

            var attachFile = messageChat.AttachFileList?
                .Where(file => file.BlobStorageUrl == command.BlobStorageUrl)
                .FirstOrDefault();

            // query other members of the conversation
            var conversation = await Mediator.Send(new GetConversationQuery
            {
                ConversationId = messageChat.ConversationId.ToString()
            });

            // notify to other members of conversation about the new update message chat
            await _hubContext.Clients.Groups(conversation.Id.ToString()).SendAsync("PinFile", new PinMessageDto
            {
                PinnedFile = new FileDto
                {
                    MessageId = messageChat.Id,
                    ConversationId = messageChat.ConversationId,
                    BlobStorageUrl = command.BlobStorageUrl,
                    IsPin = command.IsPinFile,
                    FileName = attachFile.FileName,
                    FileSize = attachFile.FileSize,
                    FileStorageName = attachFile.FileStorageName,
                    LocalUrl = attachFile.LocalUrl,
                    ThumbnailImage = attachFile.ThumbnailImage,
                },
                PinnedMessage = messageChat,
                PinnedDiscriminator = "PinnedFile"
            });

            return Ok();
        }

        private async Task<ConversationLastContentModel> GetLastConversationAsync(MessageDto msg)
        {
            var conver = await Mediator.Send(new GetConversationQuery
            {
                LoginUserId = _currentUserService.UserId,
                ConversationId = msg.ConversationId.ToString()
            });

            return new ConversationLastContentModel
            {
                Conversation = conver,
                LastMessageChatContent = new MessageChatModel
                {
                    RegUserName = msg.Name,
                    Content = msg.Message,
                    ConversationId = msg.ConversationId,
                    Created = DateTime.UtcNow
                }
            };
        }

        private CloudTable GetConnectionTable()
        {
            var storageAccount =
                CloudStorageAccount.Parse(_config["BlobStorage:ConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference("connection");
        }

        private TableQuerySegment<ConnectionEntity> GetConnectionEntitys(string key)
        {
            var table = GetConnectionTable();
            table.CreateIfNotExistsAsync();

            var query = new TableQuery<ConnectionEntity>()
                .Where(TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                key));

            return table.ExecuteQuerySegmentedAsync(query, null).Result;
        }

        private List<string> GetConnectionIds(string key)
        {
            List<string> result = new List<string>();

            foreach (var entity in GetConnectionEntitys(key))
            {
                result.Add(entity.RowKey);
            }

            return result;
        }

        private async Task CreateNotification(MessageDto msg, Guid messageId, string notifyType, ICollection<UserModel> toUser)
        {
            await Mediator.Send(new CreateNotificationCommand()
            {
                RegUserId = msg.UserId,
                RegUserName = msg.Name,
                MessageContent = msg.Message,
                ConversationId = msg.ConversationId.ToString(),
                MessageId = messageId.ToString(),
                ToUser = toUser,
                Type = notifyType,
                TeamId = msg.TeamId
            });
            // Method intentionally left empty.
        }
    }
}
