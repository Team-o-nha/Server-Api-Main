using ColabSpace.Application.Teams.Commands.CreateTeam;
using ColabSpace.Application.UnitTests.Common;
using Shouldly;
using Xunit;

namespace ColabSpace.Application.UnitTests.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandValidatorTests : CommandTestBase
    {
        [Fact]
        public void IsValid_ShouldBeTrue_WhenNameIsNotNull()
        {
            var command = new CreateTeamCommand
            {
                Name = "Not Null < 200 byte"
            };

            var validator = new CreateTeamCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void IsValid_ShouldBeFalse_WhenNameIsNull()
        {
            var command = new CreateTeamCommand
            {
                Name = null
            };

            var validator = new CreateTeamCommandValidator();

            var result = validator.Validate(command);

            result.IsValid.ShouldBe(false);
        }
    }
}
