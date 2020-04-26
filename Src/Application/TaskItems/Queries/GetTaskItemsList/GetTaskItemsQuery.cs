using ColabSpace.Application.TaskItems.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.TaskItems.Queries.GetTaskItemsList
{
    public class GetTaskItemsQuery : IRequest<IEnumerable<TaskItemModel>>
    {
        public Guid TeamId { get; set; }

        public string keyword { get; set; }

        public int? pageIndex { get; set; }
    }
}
