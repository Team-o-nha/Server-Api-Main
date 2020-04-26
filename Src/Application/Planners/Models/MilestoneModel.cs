using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.Common.Models;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Planners.Models
{
    public class MilestoneModel : IMapFrom<Milestone>
    {
        public string Title { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public ICollection<Guid> TaskIds { get; set; }

        public ICollection<TaskItemModel> Tasks { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Milestone, MilestoneModel>()
                .ForMember(x => x.Tasks, opt => opt.Ignore());
        }
    }
}
