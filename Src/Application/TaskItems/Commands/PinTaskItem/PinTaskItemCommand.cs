using ColabSpace.Application.TaskItems.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.TaskItems.Commands.PinTaskItem
{
    public class PinTaskItemCommand : IRequest<TaskItemModel>
    {
        public Guid Id { get; set; }
        public bool IsPin { get; set; }
    }
}
