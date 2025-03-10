namespace AS_2025.ApplicationServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<DepartmentService>();
        services.AddTransient<EmployeeService>();
        services.AddTransient<TeamService>();

        return services;
    }
}