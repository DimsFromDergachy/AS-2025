using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Task = AS_2025.Domain.Entities.Task;

namespace AS_2025.Common;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DatabaseFacade Database { get; }

    public DbSet<Trait> Traits { get; }

    public DbSet<Client> Clients { get; }

    public DbSet<ContactPerson> ContactPersons { get; }

    public DbSet<Department> Departments { get; }

    public DbSet<Employee> Employees { get; }

    public DbSet<Project> Projects { get; }

    public DbSet<Task> Tasks { get; }

    public DbSet<Team> Teams { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}