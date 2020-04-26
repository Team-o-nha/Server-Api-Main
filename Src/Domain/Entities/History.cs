using System;

namespace ColabSpace.Domain.Entities
{
    public class History
    {
        public String Title { get; set; }

        public String Type { get; set; }

        public String Content { get; set; }

        public DateTime Date { get; set; }

        public String UserName { get; set; }

        public String UserId { get; set; }
    }
}
