using Microsoft.AspNetCore.Identity;

namespace AS_2025.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.CreateVersion7();
}