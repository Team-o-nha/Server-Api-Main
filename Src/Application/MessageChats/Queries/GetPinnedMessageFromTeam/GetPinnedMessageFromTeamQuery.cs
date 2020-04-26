using ColabSpace.Application.Common.Models;
using ColabSpace.Application.MessageChats.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Queries.GetPinnedMessageFromTeam
{
    public class GetPinnedMessageFromTeamQuery : IRequest<IEnumerable<PinMessageDto>>
    {
        public Guid TeamId { get; set; }
        public int? PageIndex { get; set; }
    }
}
