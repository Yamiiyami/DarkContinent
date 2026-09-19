using System.ComponentModel.DataAnnotations;
using DarkContinent.DTOs.Requests;
using DarkContinent.Models;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodController : BaseApiController
{
    private readonly FoodService _foodService;

    public FoodController(FoodService foodService)
    {
        _foodService = foodService;
    }

    [HttpGet("get-list")]
    public async Task<ActionResult<List<Food>>> GetPageAsync([Range(1, 100)] int page = 1, int pageSize = 10)
    {
        var foods = await _foodService.GetPageAsync(page, pageSize);
        return SuccessResponse("Foods retrieved successfully.", foods);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateAsync(CreateFoodRequest food)
    {
        await _foodService.CreateAsync(food);
        return SuccessResponse("Foods retrieved successfully.");
    }

    [HttpPost("update")]
    public async Task<ActionResult> UpdateAsync(UpdateFoodRequest food)
    {
        await _foodService.UpdateAsync(food);
        return SuccessResponse("Foods retrieved successfully.");
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool result = await _foodService.DeleteAsync(id);
        
        if (result)
        {
            return SuccessResponse("Delete Success", 200);
        }
        else
        {
            return ErrorResponse("Delete Fail", 400);
        }
    }

}
