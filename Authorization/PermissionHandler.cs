using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using DarkContinent.Data;

namespace DarkContinent.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    public const string ClaimType = "permission";

    private readonly AuthDbContext _db;
    private readonly IMemoryCache _cache;

    public PermissionHandler(AuthDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? context.User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return;

        var permissions = await _cache.GetOrCreateAsync(CacheKey(userId), async e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var rolePerms = await (
                from ur in _db.UserRoles
                join rc in _db.RoleClaims on ur.RoleId equals rc.RoleId
                where ur.UserId == userId && rc.ClaimType == ClaimType
                select rc.ClaimValue!)
                .Distinct()
                .ToListAsync();

            var userPerms =  await(
                from uc in _db.UserClaims
                where uc.UserId == userId && uc.ClaimType == ClaimType
                select uc.ClaimValue!)
                .Distinct()
                .ToListAsync();
            

            var set = new HashSet<string>(rolePerms, StringComparer.OrdinalIgnoreCase);
            set.UnionWith(userPerms);
            return set;
        });

        if (permissions!.Contains(requirement.Permission))
            context.Succeed(requirement);
    }

    public static string CacheKey(string userId) => $"perm:{userId}";
}