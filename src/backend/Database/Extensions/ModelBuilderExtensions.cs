using Microsoft.EntityFrameworkCore;

namespace AS_2025.Database.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureEnumsAsStrings(this ModelBuilder modelBuilder, int maxLength = 50)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (!property.ClrType.IsEnum)
                {
                    continue;
                }

                property.SetMaxLength(maxLength);
                property.SetIsUnicode(false);
                property.SetColumnType($"varchar({maxLength})");
            }
        }
    }
}