using Microsoft.AspNetCore.Authorization;

namespace AS_2025.Common;

public class AnonymousAuthorizationPolicy : AuthorizationHandler<AnonymousAuthorizationPolicy>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnonymousAuthorizationPolicy requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}