using DarkContinent.DTOs.Requests;
using DarkContinent.DTOs.Responses;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetTypeController : BaseApiController
{
    private readonly PetTypeService _petTypeService;

    public PetTypeController(PetTypeService petTypeService)
    {
        _petTypeService = petTypeService;
    }

    [HttpGet("get-list")]
    public async Task<ActionResult<List<PetTypeResponse>>> GetListAsync()
    {
        return SuccessResponse("get successfully", await _petTypeService.GetPageAsync());
    }

    [HttpPost("create")]
    public async Task<ActionResult<PetTypeResponse>> CreateAsync(CreatePetTypeRequest request)
    {
        var result = await _petTypeService.CreateAsync(request);
        if (result != null)
            return SuccessResponse("create successfully", result);

        return ErrorResponse("create failed");
    }

    [HttpPost("update")]
    public async Task<ActionResult<PetTypeResponse>> UpdateAsync(UpdatePetTypeRequest request)
    {
        var result = await _petTypeService.UpdateAsync(request);
        if (result)
            return SuccessResponse("update successfully", result);
        return ErrorResponse("update failed");
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await _petTypeService.DeleteAsync(id);
        if (result)
            return SuccessResponse("delete successfully");
        return ErrorResponse("delete failed");

    }
}