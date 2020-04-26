using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Conversations.Commands.CreateConversation;
using ColabSpace.Application.Conversations.Commands.DeleteChannelConversation;
using ColabSpace.Application.Conversations.Commands.HideConversation;
using ColabSpace.Application.Conversations.Commands.ReadConversation;
using ColabSpace.Application.Conversations.Commands.UpdateConversationName;
using ColabSpace.Application.Conversations.Models;
using ColabSpace.Application.Conversations.Queries.GetConversation;
using ColabSpace.Application.Conversations.Queries.GetConversationsList;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatList;
using ColabSpace.Application.TaskItems.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColabSpace.Application.Conversations.Commands.CreateChannelConversation;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class ConversationController : ApiController
    {
        private readonly IBlobStorageService _blobStorageService;

        public ConversationController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpPost("CreateOrGet")]
        public async Task<ActionResult<ConversationMessageModel>> CreateOrGet(CreateConversationCommand command)
        {
            try
            {
                var conversation = await Mediator.Send(new GetConversationQuery
                {
                    MembersId = command.Members.Select(member => member.UserId.ToString()).ToList()
                });


                if (conversation == null)
                {
                    conversation = await Mediator.Send(command);
                }

                var messageChats = await Mediator.Send(new GetMessageChatsQuery
                {
                    ConversationId = conversation.Id,
                    PageIndex = 1
                });

                return new ConversationMessageModel
                {
                    Conversation = conversation,
                    LstMessageChat = messageChats.ToList()
                };
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<ConversationMessageModel>> Create(CreateConversationCommand command)
        {
            try
            {
                var conversation = await Mediator.Send(command);

                return new ConversationMessageModel
                {
                    Conversation = conversation,
                    LstMessageChat = new List<MessageChatModel>()
                };
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateChannel")]
        public async Task<ActionResult<ConversationModel>> CreateChannel(CreateChannelConversationCommand command)
        {
            try
            {
                var conversation = await Mediator.Send(command);

                return conversation;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAll/{UserId}")]
        public async Task<IEnumerable<ConversationModel>> GetAllConversationByUserId(string UserId, [FromQuery] string keyword, [FromQuery] Guid TeamId)
        {
            var conversations = await Mediator.Send(new GetConversationsQuery { UserId = new Guid(UserId), ConversationName = keyword, TeamId = TeamId});
            return conversations.OrderBy(x => x.Name);
        }

        [HttpGet("{conversationId}")]
        public async Task<ConversationMessageModel> GetConversationById(string conversationId, [FromQuery] int? pageIndex)
        {
            var conversation = await Mediator.Send(new GetConversationQuery
            {
                ConversationId = conversationId
            });

            var messageChats = await Mediator.Send(new GetMessageChatsQuery
            {
                ConversationId = conversation.Id,
                PageIndex = pageIndex ?? 1
            });

            return new ConversationMessageModel
            {
                Conversation = conversation,
                LstMessageChat = messageChats.ToList()
            };
        }

        [HttpPost("Hide/{conversationId}")]
        public async Task<ActionResult> Hide(string conversationId)
        {

            await Mediator.Send(new HideConversationCommand()
            {
                ConversationId = conversationId
            });

            return NoContent();
        }

        [HttpPut("{conversationId}/name")]
        public async Task<ActionResult> UpdateConversationName(string conversationId, UpdateConversationNameCommand command)
        {
            if (!conversationId.Equals(command.Id.ToString()))
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("Read/{conversationId}")]
        public async Task<ActionResult> Read(string conversationId)
        {

            await Mediator.Send(new ReadConversationCommand()
            {
                ConversationId = conversationId
            });

            return NoContent();
        }

        [HttpDelete("{conversationId}")]
        public async Task<ActionResult> Delete(string conversationId)
        {
            // query all file in message chat
            var messageChats = await Mediator.Send(new GetMessageChatsQuery { ConversationId = new Guid(conversationId) });

            // delete conversation
            await Mediator.Send(new DeleteChannelConversationCommand { ConversationId = conversationId });

            // delete files in message chats
            foreach (MessageChatModel messageChat in messageChats)
            {
                // delete file in blob
                if (messageChat.AttachFileList is var attachFileList)
                {
                    foreach (AttachFileModel file in attachFileList)
                    {
                        await _blobStorageService.DeleteBlobData(file.BlobStorageUrl);
                    }
                }
            }

            return Ok();
        }

        [HttpPost("GetConversationByMembers")]
        public async Task<ActionResult<ConversationMessageModel>> GetConversationByMembers(CreateConversationCommand command)
        {
            try
            {
                var conversation = await Mediator.Send(new GetConversationQuery
                {
                    MembersId = command.Members.Select(member => member.UserId.ToString()).ToList()
                });


                if (conversation == null)
                {
                    return new ConversationMessageModel
                    {
                        Conversation = new ConversationModel(),
                        LstMessageChat = new List<MessageChatModel>(),
                    };
                }

                var messageChats = await Mediator.Send(new GetMessageChatsQuery
                {
                    ConversationId = conversation.Id,
                    PageIndex = 1
                });

                return new ConversationMessageModel
                {
                    Conversation = conversation,
                    LstMessageChat = messageChats.ToList()
                };
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
