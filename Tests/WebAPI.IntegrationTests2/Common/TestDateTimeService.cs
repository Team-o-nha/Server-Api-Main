using ColabSpace.Domain.Interfaces;
using System;

namespace ColabSpace.WebAPI.IntegrationTests2.Common
{
    public class TestDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
