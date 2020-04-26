using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
