using ColabSpace.Application.TaskItems.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.TaskItems.Queries.GetPinnedTaskItemList
{
    public class GetPinnedTaskItemListQuery : IRequest<IEnumerable<TaskItemModel>>
    {
        public Guid TeamId { get; set; }

        public int? PageIndex { get; set; }
    }
}
