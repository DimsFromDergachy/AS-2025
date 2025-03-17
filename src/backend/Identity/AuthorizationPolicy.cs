namespace AS_2025.Identity;

public static class AuthorizationPolicy
{
    public const string Anonymous = "anonymous";

    public const string Authenticated = "authenticated";

    public const string RoleIsAdministrator = "roleIsAdministrator";

    public const string RoleIsManager = "roleIsManager";

    public const string RoleIsUser = "roleIsUser";
}