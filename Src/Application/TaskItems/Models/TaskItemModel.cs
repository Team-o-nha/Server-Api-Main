using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.TaskItems.Models
{
    public class TaskItemModel : IMapFrom<TaskItem>
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }

        public string Description { get; set; }

        public UserModel CreatedBy { get; set; }

        public UserModel Assignee { get; set; }

        public Guid TeamId { get; set; }

        public ICollection<AttachFileModel> AttachFiles { get; set; }

        public UserModel Leader { get; set; }

        public DateTime? Deadline { get; set; }

        public ICollection<TagModel> Tags { get; set; }

        public ICollection<History> Histories { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public ICollection<RelatedObjectModel> Relations { get; set; }

        public bool? IsPin { get; set; }

        public DateTime? PinnedDate { get; set; }
    }
}
