using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;
using System;

namespace ColabSpace.Application.Teams.Models
{
    public class UserModel : IMapFrom<User>
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
        public string TeamRole { get; set; }
        public bool isHidden { get; set; }
        public DateTime LastSeenTime { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserModel>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(s => Guid.Parse(s.UserOid)))
                .ReverseMap();
            profile.CreateMap<UserModel, User>()
                .ForMember(x => x.UserOid, opt => opt.MapFrom(s => s.UserId.ToString()));
        }
    }
}
