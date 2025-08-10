using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.HasKey(cu => cu.Id);

        builder.HasOne(cu => cu.Chat)
            .WithMany(c => c.ChatUsers)
            .HasForeignKey(cu => cu.ChatId);

        builder.HasOne(cu => cu.User)
            .WithMany(u => u.ChatUsers)
            .HasForeignKey(cu => cu.UserId);
    }
}