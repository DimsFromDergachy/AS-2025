using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AS_2025.Database.Configuration;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion<GuidToStringConverter>();

        builder.HasOne(c => c.AccountManager)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Contacts)
            .WithOne(cp => cp.Client)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Projects)
            .WithOne(p => p.Client)
            .OnDelete(DeleteBehavior.Restrict);
    }
}