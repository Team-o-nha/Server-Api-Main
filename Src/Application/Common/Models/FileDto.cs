using System;
using System.Collections.Generic;

namespace ColabSpace.Application.Common.Models
{
    public class FileDto
    {
        public string FileName { get; set; }

        public string FileStorageName { get; set; }

        public long FileSize { get; set; }

        public string LocalUrl { get; set; }

        public string BlobStorageUrl { get; set; }

        public string ThumbnailImage { get; set; }

        public Guid? ConversationId { get; set; }

        public Guid? MessageId { get; set; }

        public bool? IsPin { get; set; }
    }

    public class FileListDto
    {
        public FileListDto()
        {
            Size = 0;
            Files = new List<FileDto>();
        }

        public int Count { get; set; }

        public long Size { get; set; }

        public string TargetFilePath { get; set; }

        public List<FileDto> Files { get; set; }

    }
}
