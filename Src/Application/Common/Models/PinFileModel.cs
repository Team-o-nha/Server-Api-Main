using ColabSpace.Application.TaskItems.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Common.Models
{
    public class PinFileModel
    {
        public Guid ConversationId { get; set; }
        public Guid MessageId { get; set; }
        public string BlobStorageUrl { get; set; }
        public string FileName { get; set; }
        public string FileStorageName { get; set; }
        public string LocalUrl { get; set; }
        public bool? IsPin { get; set; }
        public long FileSize { get; set; }
    }
}
