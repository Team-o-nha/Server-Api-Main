using ColabSpace.Domain.Interfaces;
using System;

namespace ColabSpace.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;

        public int CurrentYear => DateTime.UtcNow.Year;
    }
}
