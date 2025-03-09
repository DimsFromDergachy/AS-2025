using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AS_2025.Database.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion<GuidToStringConverter>();

        builder.HasIndex(x => x.ExternalId).IsUnique();

        builder.HasOne(d => d.Head)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(d => d.Teams)
            .WithOne(t => t.Department)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Employees)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);
    }
}