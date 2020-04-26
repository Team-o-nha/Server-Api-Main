using System;
using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Infrastructure.Persistence;
using Xunit;

namespace ColabSpace.Application.UnitTests.Common
{
    public sealed class QueryTestFixture : IDisposable
    {
        public ColabSpaceDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public QueryTestFixture()
        {
            Context = ColabSpaceDbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            ColabSpaceDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}