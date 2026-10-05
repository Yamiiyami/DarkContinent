using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;

public class RoleService
{
    private readonly AuthDbContext _authContext;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(AuthDbContext authContext, RoleManager<IdentityRole> roleManager)
    {
        _authContext = authContext;
        _roleManager = roleManager;
    }

    public async Task<List<IdentityRole>> GetList(int page = 1, int pageSize = 10)
    {
        return await _authContext.Roles.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task create(CreateRoleRequest request)
    {
        var role = new IdentityRole{ Name = request.Name };
       await _roleManager.CreateAsync(role);
    }

    public async Task update(UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);
        if (role is null)
            throw new Exception("Role not found");
        if(role.ConcurrencyStamp != request.ConcurrencyStamp)
            throw new Exception("Role has been modified by another user");

        role.Name = request.Name;
        await _roleManager.UpdateAsync(role);
    }

    public async Task delete(DeleteRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);
        if (role == null)
            throw new Exception("Role not found");

        await _roleManager.DeleteAsync(role);
    }



}