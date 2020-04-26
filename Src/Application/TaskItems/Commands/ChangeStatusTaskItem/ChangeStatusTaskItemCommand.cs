using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.TaskItems.Commands.ChangeStatusTaskItem
{
    public class ChangeStatusTaskItemCommand : IRequest
    {
        public Guid Id { get; set; }

        public int Status { get; set; }

        public UserModel Assignee { get; set; }
    }
}
