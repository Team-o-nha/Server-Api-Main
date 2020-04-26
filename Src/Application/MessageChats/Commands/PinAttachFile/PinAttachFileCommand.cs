using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.MessageChats.Commands.PinAttachFile
{
    public class PinAttachFileCommand : IRequest
    {
        public Guid MessageId { get; set; }
        public string BlobStorageUrl { get; set; }
        public bool IsPinFile { get; set; }
    }
}
