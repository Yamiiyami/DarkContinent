using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : BaseApiController
{

    private readonly RoleService _roleService;
    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet("get-list")]

    public async Task<ActionResult> GetList(int page = 1, int pageSize = 10)
    {
        var roles = await _roleService.GetList(page, pageSize);
        return SuccessResponse("Roles retrieved successfully", roles);
    }

    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] CreateRoleRequest request)
    {
        await _roleService.create(request);
        return SuccessResponse("Role created successfully");
    }

    [HttpPost("update")]
    public async Task<ActionResult> Update([FromBody] UpdateRoleRequest request)
    {
        await _roleService.update(request);
        return SuccessResponse("Role updated successfully");
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> Delete([FromBody] DeleteRoleRequest request)
    {
        await _roleService.delete(request);
        return SuccessResponse("Role deleted successfully");
    }

   
}
