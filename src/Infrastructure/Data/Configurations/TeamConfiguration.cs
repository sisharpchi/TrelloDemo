using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Description)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.HasOne(t => t.Chat)
            .WithOne(c => c.Team)
            .HasForeignKey<Team>(t => t.ChatId);
    }
}