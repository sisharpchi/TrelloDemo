using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
{
    public void Configure(EntityTypeBuilder<UserTeam> builder)
    {
        builder.HasKey(ut => ut.Id);

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTeams)
            .HasForeignKey(ut => ut.UserId);

        builder.HasOne(ut => ut.Team)
            .WithMany(t => t.UserTeams)
            .HasForeignKey(ut => ut.TeamId);
    }
}