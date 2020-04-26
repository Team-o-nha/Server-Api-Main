//using ColabSpace.Application.HelpItems.Models;
//using ColabSpace.Application.HelpItems.Queries.GetAllHelpItem;
//using ColabSpace.WebAPI.IntegrationTests.Common;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.WebAPI.IntegrationTests.Controllers.HelpItem
//{
//    public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
//    {
//        private readonly CustomWebApplicationFactory<Startup> _factory;

//        public GetAll(CustomWebApplicationFactory<Startup> factory)
//        {
//            _factory = factory;
//        }

//        [Fact]
//        public async Task GivenValidGetAllHelpItemQuery_ReturnsSuccessCode()
//        {
//            var client = await _factory.GetAuthenticatedClientAsync();

//            var response = await client.GetAsync($"/api/HelpItem");

//            response.EnsureSuccessStatusCode();
//        }
//    }
//}
