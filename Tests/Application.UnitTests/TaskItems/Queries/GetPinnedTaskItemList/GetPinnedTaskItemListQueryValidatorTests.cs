using ColabSpace.Application.TaskItems.Queries.GetPinnedTaskItemList;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.TaskItems.Queries.GetPinnedTaskItemList
{
    public class GetPinnedTaskItemListQueryValidatorTests
    {
        [Fact]
        public void IsValid_ShouldBeFalse_WhenTeamIdIsEmpty()
        {
            var query = new GetPinnedTaskItemListQuery
            {
                TeamId = Guid.Empty
            };

            var validator = new GetPinnedTaskItemListQueryValidator();

            var result = validator.Validate(query);

            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ShouldBeTrue_QueryValid()
        {
            var query = new GetPinnedTaskItemListQuery
            {
                TeamId = Guid.NewGuid()
            };

            var validator = new GetPinnedTaskItemListQueryValidator();

            var result = validator.Validate(query);

            result.IsValid.ShouldBe(true);
        }
    }
}
