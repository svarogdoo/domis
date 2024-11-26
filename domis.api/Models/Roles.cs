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
    public static string GetName(this Roles role)
    {
        return role.ToString();
    }
}