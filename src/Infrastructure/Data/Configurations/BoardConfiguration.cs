using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasOne(b => b.Team)
            .WithMany(t => t.Boards)
            .HasForeignKey(b => b.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}