using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Application.TaskItems.Models;
using ColabSpace.Application.Teams.Models;
using ColabSpace.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ColabSpace.Application.MessageChats.Models
{
    public class MessageChatModel : IMapFrom<MessageChat>
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public Guid ConversationId { get; set; }

        public string ConversationName { get; set; }
            
        public string RegUserName { get; set; }

        public ICollection<AttachFileModel> AttachFileList { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public ICollection<ReactionModel> ReactionList { get; set; }

        public bool IsPin { get; set; }

        public DateTime PinnedDate { get; set; }

        public Guid? RelatedMessagesId { get; set; }

        public Guid? RelatedTaskId { get; set; }

        public MessageChatModel RelatedMessages { get; set; }

        public ICollection<UserModel> Mentions { get; set; }

        public string TeamId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MessageChat, MessageChatModel>()
                .ForMember(x => x.Mentions, opt => opt.Ignore())
                .ForMember(x => x.TeamId, opt => opt.Ignore());
        }
    }
}
