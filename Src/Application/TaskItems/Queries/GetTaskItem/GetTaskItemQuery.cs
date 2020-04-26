using ColabSpace.Application.TaskItems.Models;
using MediatR;
using System;

namespace ColabSpace.Application.TaskItems.Queries.GetTaskItem
{
    public class GetTaskItemQuery : IRequest<TaskItemModel>
    {
        public Guid TaskItemId { get; set; }
    }
}
