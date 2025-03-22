using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Database;

public static class DbSetExtensions
{
    public static IQueryable<Department> Full(this DbSet<Department> dbSet)
    {
        return dbSet
            .Include(x => x.Head)
            .Include(x => x.Teams)
            .Include(x => x.Employees);
    }
}