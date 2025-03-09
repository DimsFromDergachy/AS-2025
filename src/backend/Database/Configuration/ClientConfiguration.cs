using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AS_2025.Database.Configuration;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
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