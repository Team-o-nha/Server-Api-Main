using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommand : IRequest
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public int Status { get; set; }

        public String Description { get; set; }

        public UserModel CreatedBy { get; set; }

        public UserModel Assignee { get; set; }

        public Guid TeamId { get; set; }

        public ICollection<AttachFileModel> AttachFiles { get; set; }

        public DateTime? Deadline { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        public ICollection<RelatedObjectModel> Relations { get; set; }
    }
}
