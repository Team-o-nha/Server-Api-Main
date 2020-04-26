using ColabSpace.Application.MessageChats.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColabSpace.Application.Common.Models
{
    public class PinMessageDto
    {
        public MessageChatModel PinnedMessage { get; set; }
        public FileDto PinnedFile { get; set; }
        public string PinnedDiscriminator { get; set; }
    }
}
