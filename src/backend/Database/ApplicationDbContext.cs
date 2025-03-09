using AS_2025.Common;
using AS_2025.Domain.Entities;
using AS_2025.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = AS_2025.Domain.Entities.Task;

namespace AS_2025.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Trait> Traits { get; set; } = null!;

    public DbSet<Client> Clients { get; set; } = null!;

    public DbSet<ContactPerson> ContactPersons { get; set; } = null!;

    public DbSet<Department> Departments { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Project> Projects { get; set; } = null!;

    public DbSet<Task> Tasks { get; set; } = null!;

    public DbSet<Team> Teams { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IAssemblyMarker).Assembly);
    }
}