﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(255);

        builder.HasData(
            new UserRole { Id = 1, Name = "User", Description = "Oddiy foydalanuvchi" },
            new UserRole { Id = 2, Name = "Admin", Description = "Administrator" },
            new UserRole { Id = 3, Name = "SuperAdmin", Description = "Bosh administrator" }
        );
    }
}