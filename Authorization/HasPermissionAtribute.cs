using Microsoft.AspNetCore.Authorization;

namespace DarkContinent.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public const string Prefix = "PERMISSION:";
    public HasPermissionAttribute(string permission) => Policy = Prefix + permission;
}
