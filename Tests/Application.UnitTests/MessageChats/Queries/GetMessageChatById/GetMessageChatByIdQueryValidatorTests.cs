using ColabSpace.Application.MessageChats.Queries.GetMessageChatById;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ColabSpace.Application.UnitTests.MessageChats.Queries.GetMessageChatById
{
    public class GetMessageChatByIdQueryValidatorTests
    {
        [Fact]
        public void IsValid_ShouldBeFalse_WhenMessageChatIdIsEmpty()
        {
            var query = new GetMessageChatByIdQuery
            {
                MessageChatId = Guid.Empty
            };

            var validator = new GetMessageChatByIdQueryValidator();

            var result = validator.Validate(query);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeTrue_QueryValid()
        {
            var query = new GetMessageChatByIdQuery
            {
                MessageChatId = Guid.NewGuid()
            };

            var validator = new GetMessageChatByIdQueryValidator();

            var result = validator.Validate(query);

            result.IsValid.ShouldBe(true);
        }
    }
}
