using DarkContinent.Authorization;
using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DarkContinent.Services;

public class RoleClaimService
{
    private readonly AuthDbContext _db;
    private readonly IMemoryCache _cache;

    public RoleClaimService(AuthDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<IdentityRoleClaim<string>>?> GetByRoleIdAsync(string roleId)
    {
        var roleClaim = await _db.RoleClaims.Where(r => r.RoleId == roleId).ToListAsync();
        return roleClaim;
    }

    public async Task<bool> Create(CreateRoleClaimRequest request)
    {
        ValidatePermission(request.ClaimType, request.ClaimValue);

        var roleExists = await _db.Roles.AnyAsync(role => role.Id == request.RoleId);
        if (!roleExists)
            throw new ArgumentException("Role not found");

        var duplicate = await _db.RoleClaims.AnyAsync(claim =>
            claim.RoleId == request.RoleId &&
            claim.ClaimType == PermissionHandler.ClaimType &&
            claim.ClaimValue == request.ClaimValue);
        if (duplicate)
            return false;

        var roleClaim = new IdentityRoleClaim<string>
        {
            RoleId = request.RoleId,
            ClaimType = request.ClaimType,
            ClaimValue = request.ClaimValue
        };

        _db.RoleClaims.Add(roleClaim);
        var result = await _db.SaveChangesAsync();
        await InvalidateRoleUsersAsync(request.RoleId);
        return result > 0;
    }

    public async Task Update(UpdateRoleClaimRequest request)
    {
        var roleClaim = await _db.RoleClaims.FirstOrDefaultAsync(rc => rc.Id == int.Parse(request.Id));
        if (roleClaim is null) return;

        ValidatePermission(request.ClaimType, request.ClaimValue);
        var previousRoleId = roleClaim.RoleId;

        var roleExists = await _db.Roles.AnyAsync(role => role.Id == request.RoleId);
        if (!roleExists)
            throw new ArgumentException("Role not found");

        var duplicate = await _db.RoleClaims.AnyAsync(claim =>
            claim.Id != roleClaim.Id &&
            claim.RoleId == request.RoleId &&
            claim.ClaimType == PermissionHandler.ClaimType &&
            claim.ClaimValue == request.ClaimValue);
        if (duplicate)
            throw new ArgumentException("Permission already exists for this role");

        roleClaim.RoleId = request.RoleId;
        roleClaim.ClaimType = request.ClaimType;
        roleClaim.ClaimValue = request.ClaimValue;

        await _db.SaveChangesAsync();
    await InvalidateRoleUsersAsync(previousRoleId, request.RoleId);
    }

    public async Task Delete(int id)
    {
        var roleClaim = await _db.RoleClaims.FirstOrDefaultAsync(rc => rc.Id == id);
        if (roleClaim is null) return;

        var roleId = roleClaim.RoleId;

        _db.RoleClaims.Remove(roleClaim);
        await _db.SaveChangesAsync();
        await InvalidateRoleUsersAsync(roleId);
    }

    private static void ValidatePermission(string claimType, string claimValue)
    {
        if (!string.Equals(claimType, PermissionHandler.ClaimType, StringComparison.Ordinal))
            throw new ArgumentException($"Only '{PermissionHandler.ClaimType}' claims are supported");

        if (string.IsNullOrWhiteSpace(claimValue))
            throw new ArgumentException("Permission cannot be empty");
    }

    private async Task InvalidateRoleUsersAsync(params string[] roleIds)
    {
        var userIds = await _db.UserRoles
            .Where(userRole => roleIds.Contains(userRole.RoleId))
            .Select(userRole => userRole.UserId)
            .Distinct()
            .ToListAsync();

        foreach (var userId in userIds)
            _cache.Remove(PermissionHandler.CacheKey(userId));
    }


}