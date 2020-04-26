using ColabSpace.Application.TaskItems.Models;
using MediatR;

namespace ColabSpace.Application.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommand : IRequest<TaskItemModel>
    {
        public string Id { get; set; }
    }
}
