using DarkContinent.Authorization;
using DarkContinent.DTOs.Requests;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleClaimController : BaseApiController
{
    private readonly RoleClaimService _roleClaimService;
    public RoleClaimController(RoleClaimService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }

    [HttpGet("get-list-by-role/{RoleId}")]
    [HasPermission("role-claim.read")]
    public async Task<IActionResult> GetByRoleId(string roleId)
    {
        var roleClaims = await _roleClaimService.GetByRoleIdAsync(roleId);
        return SuccessResponse("Lấy danh sách quyền của vai trò thành công", roleClaims);
    }

    [HttpPost("create")]
    [HasPermission("role-claim.create")]
    public async Task<IActionResult> Create(CreateRoleClaimRequest request)
    {
        var result = await _roleClaimService.Create(request);
        if (result)
        {
            return SuccessResponse("Tạo quyền cho vai trò thành công");
        }
        else
        {
            return ErrorResponse("Tạo quyền cho vai trò thất bại");
        }
    }

    [HttpPost("update")]
    [HasPermission("role-claim.update")]
    public async Task<IActionResult> Update(UpdateRoleClaimRequest request)
    {
        await _roleClaimService.Update(request);
        return SuccessResponse("Cập nhật quyền cho vai trò thành công");
    }

    [HttpDelete("delete/{id}")]
    [HasPermission("role-claim.delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roleClaimService.Delete(id);
        return SuccessResponse("Xóa quyền của vai trò thành công");
    }

}