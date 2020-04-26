//using ColabSpace.Application.HelpItems.Models;
//using ColabSpace.Application.HelpItems.Queries.GetAllHelpItem;
//using ColabSpace.Application.UnitTests.Common;
//using ColabSpace.Domain.Entities;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.Application.UnitTests.HelpItems.Queries.GetAllHelpItem
//{
//    public class GetAllHelpItemQueryHandlerTests : QueryTestBase
//    {

//        public GetAllHelpItemQueryHandlerTests()
//        {
//        }

//        [Fact]
//        public async Task GetHelpItem_ListIsEmpty()
//        {
//            // target handler
//            var sut = new GetAllHelpItemQueryHandler(Context, Mapper);

//            // query with invalid CommonNo
//            var result = await sut.Handle(new GetAllHelpItemQuery(), CancellationToken.None);

//            // assertion
//            result.ShouldBeOfType<List<HelpItemModel>>();
//            // not null and size is 0
//            result.Count().ShouldBe(0);
//        }

//        [Fact]
//        public async Task GetHelpItem_ListOneElement()
//        {
//            // Arrange
//            Context.HelpItems.AddRange(new[] {
//                new HelpItem()
//                {
//                    Id = new Guid(),
//                    Name = "Topic",
//                    Description = string.Empty,
//                    Content = new AttachFile()
//                    {
//                        FileName = "test.a",
//                        FileSize = 10,
//                        BlobStorageUrl = null,
//                        FileStorageName = "test.b",
//                    },
//                }
//            });
//            Context.SaveChanges();

//            // target handler
//            var sut = new GetAllHelpItemQueryHandler(Context, Mapper);

//            // query with 1 element
//            var result = await sut.Handle(new GetAllHelpItemQuery(), CancellationToken.None);

//            // assertion
//            result.ShouldBeOfType<List<HelpItemModel>>();
//            // not null and size is 1
//            result.Count().ShouldBe(1);
//        }
//    }
//}
