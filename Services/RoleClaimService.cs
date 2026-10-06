using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;

public class RoleClaimService
{
    private readonly AuthDbContext _db;
    public RoleClaimService(AuthDbContext db)
    {
        _db = db;
    }

    public async Task<List<IdentityRoleClaim<string>>?> GetByRoleIdAsync(string roleId)
    {
        var roleClaim = await _db.RoleClaims.Where(r => r.RoleId == roleId).ToListAsync();
        return roleClaim;
    }

    public async Task<bool> Create(CreateRoleClaimRequest request)
    {
        var roleClaim = new IdentityRoleClaim<string>
        {
            RoleId = request.RoleId,
            ClaimType = request.ClaimType,
            ClaimValue = request.ClaimValue
        };

        _db.RoleClaims.Add(roleClaim);
        var result = await _db.SaveChangesAsync();
        return result > 0;
    }

    public async Task Update(UpdateRoleClaimRequest request)
    {
        var roleClaim = await _db.RoleClaims.FirstOrDefaultAsync(rc => rc.Id == int.Parse(request.Id));
        if (roleClaim is null) return;

        roleClaim.RoleId = request.RoleId;
        roleClaim.ClaimType = request.ClaimType;
        roleClaim.ClaimValue = request.ClaimValue;

        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var roleClaim = await _db.RoleClaims.FirstOrDefaultAsync(rc => rc.Id == id);
        if (roleClaim is null) return;

        _db.RoleClaims.Remove(roleClaim);
        await _db.SaveChangesAsync();
    }


}