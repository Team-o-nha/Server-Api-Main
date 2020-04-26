using ColabSpace.Domain.Interfaces;
using System;

namespace ColabSpace.WebAPI.IntegrationTests.Common
{
    public class TestDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
