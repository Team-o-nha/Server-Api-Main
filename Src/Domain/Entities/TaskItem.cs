using System;
using System.Collections.Generic;

namespace ColabSpace.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public int? Status { get; set; }

        public String Description { get; set; }

        public User CreatedBy { get; set; }

        public User Assignee { get; set; }

        public Guid? TeamId { get; set; }

        public ICollection<AttachFile> AttachFiles { get; set; }

        public User Leader { get; set; }

        public DateTime? Deadline { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<History> Histories { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public ICollection<RelatedObject> Relations { get; set; }

        public bool? IsPin { get; set; }

        public DateTime? PinnedDate { get; set; }
    }
}
