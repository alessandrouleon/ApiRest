namespace APIRest.API.Common;

/// <summary>
/// Role name constants — use these instead of magic strings in [Authorize(Roles = ...)] attributes.
/// Values must match exactly the UserRole enum names, since the JWT claim is set via role.ToString().
/// </summary>
public static class Roles
{
    public const string Admin       = "Admin";
    public const string ClientAdmin = "ClientAdmin";
    public const string Manager     = "Manager";
    public const string Technician  = "Technician";
    public const string Leader      = "Leader";
    public const string Operator    = "Operator";

    // Shorthand combinations for reuse across controllers
    public const string AdminOrClientAdmin               = $"{Admin},{ClientAdmin}";
    public const string AdminClientAdminOrManager        = $"{Admin},{ClientAdmin},{Manager}";
    public const string AdminClientAdminManagerOrLeader  = $"{Admin},{ClientAdmin},{Manager},{Leader}";
}
