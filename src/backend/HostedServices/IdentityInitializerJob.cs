using AS_2025.Identity;
using AS_2025.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AS_2025.HostedServices;

public class IdentityInitializerJob : IChainedHostedServiceJob
{
    private const string DefaultPassword = "Password123";

    private readonly IReadOnlyCollection<UserInfo> _users = new[]
    {
        new UserInfo("admin@test.com", "admin@test.com", DefaultPassword, new[] { UserRole.Administrator }),
        new UserInfo("manager1@test.com", "manager1@test.com", DefaultPassword, new[] { UserRole.Manager }),
        new UserInfo("user1@test.com", "user1@test.com", DefaultPassword, new[] { UserRole.User }),
        new UserInfo("user2@test.com", "user2@test.com", DefaultPassword, new[] { UserRole.User })
    };

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<IdentityInitializerJob> _logger;

    public int Order => 1;

    public IdentityInitializerJob(
        IServiceProvider serviceProvider,
        ILogger<IdentityInitializerJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var options = scope.ServiceProvider.GetRequiredService<IOptions<ApplicationOptions>>();
        if (!options.Value.HostedServices.IsEnabled(HostedServicesOptions.IdentityInitializer))
        {
            return;
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationUserRole>>();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

        var roles = _users.SelectMany(x => x.Roles).Distinct();
        foreach (var userRole in roles)
        {
            if (!await roleManager.RoleExistsAsync(userRole.ToString()))
            {
                await roleManager.CreateAsync(new ApplicationUserRole { Name = userRole.ToString() });
            }
        }

        foreach (var userInfo in _users)
        {
            var user = await userManager.FindByEmailAsync(userInfo.Email);
            if (user is not null)
            {
                await userManager.DeleteAsync(user);
            }

            var identityUser = new ApplicationUser
            {
                UserName = userInfo.Username,
                Email = userInfo.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, userInfo.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(identityUser, userInfo.Roles.Select(x => x.ToString()));
            }
        }
    }

    private record UserInfo(string Email, string Username, string Password, IReadOnlyCollection<UserRole> Roles);
}