namespace domis.api.Models;

public enum Roles
{
    User,
    Admin,
    VP1,
    VP2,
    VP3,
    VP4
}

public static class RoleExtensions
{
    public static string RoleName(this Roles role)
    {
        return role.ToString();
    }
}