using AutoMapper;
using ColabSpace.Application.Common.Mappings;
using ColabSpace.Domain.Entities;
using System.Collections.Generic;

namespace ColabSpace.Application.TaskItems.Models
{
    public class AttachFileModel : IMapFrom<AttachFile>
    {
        public string FileName { get; set; }

        public string FileStorageName { get; set; }

        public long FileSize { get; set; }

        public string LocalUrl { get; set; }

        public string BlobStorageUrl { get; set; }

        public string ThumbnailImage { get; set; }

        public bool? IsPin { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AttachFile, AttachFileModel>()
                .ForMember(x => x.LocalUrl, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
