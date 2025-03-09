using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task = AS_2025.Domain.Entities.Task;

namespace AS_2025.Database.Configuration;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion<GuidToStringConverter>();

        builder.HasOne(t => t.AssignedTo)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
    }
}