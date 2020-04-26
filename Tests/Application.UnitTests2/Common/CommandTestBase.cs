using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Infrastructure.Persistence;
using System;

namespace ColabSpace.Application.UnitTests2.Common
{
    public class CommandTestBase : IDisposable
    {
        protected readonly ColabSpaceDbContext _context;
        protected readonly IMapper _mapper;

        public CommandTestBase()
        {
            _context = ColabSpaceDbContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            ColabSpaceDbContextFactory.Destroy(_context);
            GC.SuppressFinalize(this);
        }
    }
}