//using ColabSpace.WebAPI.IntegrationTests.Common;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.WebAPI.IntegrationTests.Controllers.HelpItem
//{
//    public class DeleteHelpItemControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly CustomWebApplicationFactory<Startup> _factory;

//        public DeleteHelpItemControllerTests(CustomWebApplicationFactory<Startup> factory)
//        {
//            _factory = factory;
//        }

//        [Fact]
//        public async Task GivenInvalidId_ReturnsNotFound()
//        {
//            var invalidId = Guid.NewGuid();

//            var client = await _factory.GetAuthenticatedClientAsync();

//            var response = await client.DeleteAsync($"/api/HelpItem/{invalidId}");

//            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task GivenValidId_ReturnsSuccessStatusCode()
//        {
//            var validId = new Guid("7B41389E-7C41-4594-8AF7-06F01D21F973");

//            var client = await _factory.GetAuthenticatedClientAsync();

//            var response = await client.DeleteAsync($"/api/HelpItem/{validId}");

//            response.EnsureSuccessStatusCode();
//        }
//    }
//}
