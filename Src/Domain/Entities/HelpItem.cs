using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Domain.Entities
{
    public class HelpItem
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public AttachFile Content { get; set; }
    }
}
