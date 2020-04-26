using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Infrastructure.Persistence;
using System;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ColabSpace.Application.UnitTests2.Common
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
            GC.SuppressFinalize(this);
        }
    }

    //[CollectionDefinition("QueryCollection", DisableParallelization = true)]
    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }

    [CollectionDefinition("QueryCollection2", DisableParallelization = true)]
    public class QueryCollection2 : ICollectionFixture<QueryTestFixture> { }

    [CollectionDefinition("QueryCollection3", DisableParallelization = true)]
    public class QueryCollection3 : ICollectionFixture<QueryTestFixture> { }
}