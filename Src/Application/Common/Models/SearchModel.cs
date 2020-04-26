using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Common.Models
{
    public class SearchModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool isTeam { get; set; }
    }
}
