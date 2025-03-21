﻿using AS_2025.Database.Extensions;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AS_2025.Database.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion<GuidToStringConverter>();

        builder.Property(e => e.Skills)
            .HasColumnType("jsonb")
            .HasJsonConversion();

        builder.HasOne(e => e.Manager)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}