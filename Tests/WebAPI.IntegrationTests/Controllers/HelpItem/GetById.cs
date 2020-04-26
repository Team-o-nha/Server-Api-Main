//using ColabSpace.Application.HelpItems.Models;
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
//    public class GetById : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly CustomWebApplicationFactory<Startup> _factory;

//        public GetById(CustomWebApplicationFactory<Startup> factory)
//        {
//            _factory = factory;
//        }

//        [Fact]
//        public async Task GivenInvalidHelpItemId_ReturnsNotFound()
//        {
//            var client = await _factory.GetAuthenticatedClientAsync();
//            var invalidHelpItemId = Guid.NewGuid();

//            var response = await client.GetAsync($"/api/HelpItem/{invalidHelpItemId}");

//            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task GivenValidHelpItemId_ReturnsHelpItemModel()
//        {
//            var client = await _factory.GetAuthenticatedClientAsync();
//            var validHelpItemId = new Guid("7B41389E-7C41-4594-8AF7-06F01D21F973");

//            var response = await client.GetAsync($"/api/HelpItem/{validHelpItemId}");

//            response.EnsureSuccessStatusCode();

//            var vm = await IntegrationTestHelper.GetResponseContent<HelpItemModel>(response);

//            vm.ShouldBeOfType<HelpItemModel>();
//            vm.Id.ShouldBe(validHelpItemId);
//        }
//    }
//}
