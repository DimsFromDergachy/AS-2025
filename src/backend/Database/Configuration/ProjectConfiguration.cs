using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AS_2025.Database.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(p => p.ProjectManager)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.AssignedTeams)
            .WithMany(t => t.AssignedProjects)
            .UsingEntity(j => j.ToTable("ProjectTeams"));

        builder.HasMany(p => p.AdditionalMembers)
            .WithMany()
            .UsingEntity(j => j.ToTable("ProjectAdditionalMembers"));

        builder.HasMany(p => p.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}