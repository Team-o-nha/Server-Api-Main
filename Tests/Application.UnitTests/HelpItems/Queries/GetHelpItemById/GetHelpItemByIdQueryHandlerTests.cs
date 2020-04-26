//using ColabSpace.Application.Common.Exceptions;
//using ColabSpace.Application.HelpItems.Models;
//using ColabSpace.Application.HelpItems.Queries.GetHelpItemById;
//using ColabSpace.Application.UnitTests.Common;
//using ColabSpace.Domain.Entities;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace ColabSpace.Application.UnitTests.HelpItems.Queries.GetHelpItemById
//{
//    public class GetHelpItemByIdQueryHandlerTests : QueryTestBase
//    {
//        private readonly Guid validHelpItemId = new Guid("7B41389E-7C41-4594-8AF7-06F01D21F973");
//        private readonly Guid invalidHelpItemId = new Guid();

//        public GetHelpItemByIdQueryHandlerTests()
//        {
//            // Arrange
//            Context.HelpItems.AddRange(new[] {
//                new HelpItem()
//                {
//                    Id = validHelpItemId,
//                    Name = "Topic",
//                    Description = string.Empty,
//                    Content = new AttachFile()
//                    {
//                        FileName = "test.a",
//                        FileSize = 10,
//                        BlobStorageUrl = null,
//                        FileStorageName = "test.b",
//                    }
//                }
//            });
//            Context.SaveChanges();
//        }

//        [Fact]
//        public async Task GetHelpItem_NotFound()
//        {
//            // target handler
//            var sut = new GetHelpItemByIdQueryHandler(Context, Mapper);

//            // assertion
//            await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(new GetHelpItemByIdQuery()
//            {
//                HelpItemId = invalidHelpItemId
//            }, CancellationToken.None));
//        }

//        [Fact]
//        public async Task GetHelpItem_Found()
//        {
//            // target handler
//            var sut = new GetHelpItemByIdQueryHandler(Context, Mapper);

//            var result = await sut.Handle(new GetHelpItemByIdQuery
//            {
//                HelpItemId = validHelpItemId
//            }
//            , CancellationToken.None);

//            result.ShouldBeOfType<HelpItemModel>();
//            result.Id.ShouldBe(validHelpItemId);
//        }
//    }
//}
