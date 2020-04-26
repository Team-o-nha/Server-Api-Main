using ColabSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColabSpace.Infrastructure.Persistence.Configurations
{
    public class MessageChatConfiguration : IEntityTypeConfiguration<MessageChat>
    {
        public void Configure(EntityTypeBuilder<MessageChat> builder)
        {
            builder.ToContainer("MessageChats");
            builder.OwnsMany<AttachFile>(p => p.AttachFileList);
            builder.OwnsMany<Reaction>(p => p.ReactionList);
            builder.Ignore(p => p.RelatedMessages);
            builder.HasNoDiscriminator();
        }
    }
}
