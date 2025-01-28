using AS_2025.Common;
using AS_2025.Domain.Entities;
using AS_2025.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Trait> Traits { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}