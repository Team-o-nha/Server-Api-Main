using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.UnitTests.Common
{
    public class QueryTestBase : IDisposable
    {
        public ColabSpaceDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public QueryTestBase()
        {
            Context = ColabSpaceDbContextFactory.Create();

            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }).CreateMapper();
        }

        public void Dispose()
        {
            ColabSpaceDbContextFactory.Destroy(Context);
        }
    }
}
