using DarkContinent.DTOs.Request;
using DarkContinent.DTOs.Requests;
using DarkContinent.DTOs.Responses;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityController : BaseApiController
{
    private readonly ActivityService _activityService;

    public ActivityController(ActivityService activityService)
    {
        _activityService = activityService;
    }


    [HttpGet("get-list")]
    public async Task<ActionResult<List<ActivityResponse>>> GetPageAsync(int page = 1, int pageSize = 10)
    {
        return SuccessResponse("get successfully", await _activityService.GetPageAsync(page, pageSize));
    }

    [HttpPost("create")]
    public async Task<ActionResult<ActivityResponse>> CreateAsync(CreateActivityRequest activity)
    {
        var result = await _activityService.Create(activity);
        if (result != null)
        {
            return SuccessResponse("create successfully", result);
        }
        return ErrorResponse("create failed");
    }

    [HttpPost("update")]
    public async Task<ActionResult<ActivityResponse>> UpdateAsync(UpdateActivityRequest activity)
    {
        var result = await _activityService.updateAsync(activity);
        if (result != null)
        {
            return SuccessResponse("update successfully", result);
        }
        return ErrorResponse("update failed");
    }
    
    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var result = await _activityService.DeleteAsync(id);
        if (result)
        {
            return SuccessResponse("delete successfully");
        }
        return ErrorResponse("delete failed");
    }
            

}