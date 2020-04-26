using ColabSpace.Application.Teams.Commands.UpdateTeam;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using System;
using Xunit;

namespace ColabSpace.Application.UnitTests.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandValidatorTests
    {
        private readonly Guid validId = ColabSpaceDbContextFactory.teamId1;

        [Fact]
        public void IsValid_ShouldBeTrue_WhenNameIsNotNull()
        {
            var command = new UpdateTeamCommand
            {
                Id = validId.ToString(),
                Name = "Not Null < 200 byte"
            };

            var validator = new UpdateTeamCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameIsNull()
        {
            var command = new UpdateTeamCommand
            {
                Id = validId.ToString(),
                Name = null
            };

            var validator = new UpdateTeamCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
