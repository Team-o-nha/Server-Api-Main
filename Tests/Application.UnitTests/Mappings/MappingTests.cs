using AutoMapper;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using Shouldly;
using Xunit;

namespace ColabSpace.Application.UnitTests.Mappings
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests(MappingTestsFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapTeamToTeamModel()
        {
            var entity = new Team();

            var result = _mapper.Map<TeamModel>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TeamModel>();
        }

        [Fact]
        public void ShouldMapUserToUserModel()
        {
            var entity = new User();

            var result = _mapper.Map<UserModel>(entity);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserModel>();
        }
    }
}
