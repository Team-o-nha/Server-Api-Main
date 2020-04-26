using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public int Status { get; set; }

        public string Description { get; set; }

        public UserModel CreatedBy { get; set; }

        public UserModel Assignee { get; set; }

        public Guid TeamId { get; set; }

        public ICollection<AttachFileModel> AttachFiles { get; set; }

        public DateTime? Deadline { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public ICollection<RelatedObjectModel> Relations { get; set; }
    }
}
