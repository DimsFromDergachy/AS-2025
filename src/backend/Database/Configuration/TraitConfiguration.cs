using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AS_2025.Database.Configuration;

public class TraitConfiguration : IEntityTypeConfiguration<Trait>
{
    public void Configure(EntityTypeBuilder<Trait> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion<GuidToStringConverter>();
    }
}