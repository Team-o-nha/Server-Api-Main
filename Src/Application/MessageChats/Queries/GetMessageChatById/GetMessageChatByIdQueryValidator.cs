using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Queries.GetMessageChatById
{
    public class GetMessageChatByIdQueryValidator : AbstractValidator<GetMessageChatByIdQuery>
    {
        public GetMessageChatByIdQueryValidator()
        {
            RuleFor(query => query.MessageChatId).NotEqual(Guid.Empty);
        }
    }
}
