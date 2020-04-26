using ColabSpace.Application.Common.Interfaces;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.MessageChats.Commands.CreateMessageChat;
using ColabSpace.Application.MessageChats.Commands.UpdateMessageChat;
using ColabSpace.Application.MessageChats.Models;
using ColabSpace.Application.MessageChats.Queries.GetLastContentOfConversation;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatById;
using ColabSpace.Application.MessageChats.Queries.GetMessageChatList;
using ColabSpace.Application.MessageChats.Queries.GetPinnedMessageFromTeam;
using ColabSpace.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColabSpace.WebAPI.Controllers
{
    [Authorize]
    public class MessageChatController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;
        public MessageChatController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateMessageChatCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("GetLastOf/{UserId}")]
        public async Task<IEnumerable<ConversationLastContentModel>> GetLastContentOfConversation(string UserId, [FromQuery]string teamId)
        {
            if (teamId == null || Guid.Empty == Guid.Parse(teamId))
            {
                return await Mediator.Send(new GetLastContentOfConversationQuery
                {
                    UserId = Guid.Parse(UserId),
                    TeamId = Guid.Empty
                });
            }
            else
            {
                return await Mediator.Send(new GetLastContentOfConversationQuery
                {
                    UserId = Guid.Parse(UserId),
                    TeamId = Guid.Parse(teamId)
                });
            }
        }

        /**
         * Not realtime action
         */
        [HttpPut("{messageId}")]
        public async Task<ActionResult> Update(string messageId, UpdateMessageChatCommand command)
        {
            if (!messageId.Equals(command.MessageId.ToString()))
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return Ok();
        }

        [HttpGet("GetPinnedMessageFromTeam/{teamId}")]
        public async Task<IEnumerable<PinMessageDto>> GetPinnedMessageFromTeam(string teamId, [FromQuery] int? pageIndex)
        {
            var result = await Mediator.Send(new GetPinnedMessageFromTeamQuery()
            {
                TeamId = Guid.Parse(teamId),
                PageIndex = pageIndex
            });

            return result;
        }

        [HttpGet("GetRelatedMessage/{messageId}")]
        public async Task<IEnumerable<MessageDto>> GetRelatedMessage(string messageId, [FromQuery] string relatedTaskId)
        {
            var taskId = Guid.Empty;
            if (!string.IsNullOrEmpty(relatedTaskId))
            {
                taskId = new Guid(relatedTaskId);
            }
            var messages = await Mediator.Send(new GetMessageChatsQuery
            {
                RelatedMessagesId = new Guid(messageId),
                RelatedTaskId = taskId
            });

            List<MessageDto> result = new List<MessageDto>();

            foreach (var message in messages)
            {
                result.Add(new MessageDto()
                {
                    AttachFiles = message.AttachFileList,
                    ConversationId = message.ConversationId,
                    IsPin = message.IsPin,
                    ConversationName = message.ConversationName,
                    Date = message.Created,
                    Message = message.Content,
                    MessageId = message.Id.ToString(),
                    Name = message.RegUserName,
                    UserId = message.CreatedBy,
                    Reactions = message.ReactionList,
                    Type = _currentUserService.UserId == message.CreatedBy ? "sent" : "received",
                });
            }

            return result;
        }

        [HttpGet("GetMessage/{messageId}")]
        public async Task<MessageDto> GetMessage(string messageId)
        {

            var message = await Mediator.Send(new GetMessageChatByIdQuery
            {
                MessageChatId = new Guid(messageId)
            });

            return new MessageDto()
            {
                AttachFiles = message.AttachFileList,
                ConversationId = message.ConversationId,
                IsPin = message.IsPin,
                ConversationName = message.ConversationName,
                Date = message.Created,
                Message = message.Content,
                MessageId = message.Id.ToString(),
                Name = message.RegUserName,
                UserId = message.CreatedBy,
                Reactions = message.ReactionList,
                Type = _currentUserService.UserId == message.CreatedBy ? "sent" : "received",
            };
        }

        [HttpGet("GetByConversation/{conversationId}")]
        public async Task<IEnumerable<MessageChatModel>> GetMessagesByConversationId(string conversationId, [FromQuery] int? pageIndex)
        {
            if (pageIndex == null || pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (string.IsNullOrWhiteSpace(conversationId))
            {
                return new List<MessageChatModel>();
            }
            return await Mediator.Send(new GetMessageChatsQuery
            {
                ConversationId = new Guid(conversationId),
                PageIndex = pageIndex
            });
        }
    }
}
