using DarkContinent.Authorization;
using DarkContinent.Requests;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetController : BaseApiController
{
    private readonly PetService _petService;

    public PetController(PetService petService)
    {
        _petService = petService;
    }

    [HttpPost("create")]
    [HasPermission("pet.create")]
    public async Task<IActionResult> CreatePet(CreatePetRequest request)
    {
        try
        {
            await _petService.CreatePet(request);
            return SuccessResponse("Tạo thú cưng thành công");
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex.Message);
        }
    }

    [HttpGet("get-alive-pet")]
    [HasPermission("pet.read")]
    public async Task<IActionResult> GetAlivePet()
    {
        var pet = await _petService.GetCurrentPet();
        if (pet != null)
        {
            return SuccessResponse("Lấy thú cưng đang sống thành công", pet);
        }
        else
        {
            return ErrorResponse("Không có thú cưng đang sống");
        }
    }

    [HttpPost("start-game")]
    [HasPermission("pet.play")]
    public async Task<IActionResult> StartGame()
    {
        await _petService.StartGame();
        return SuccessResponse("Bắt đầu trò chơi thành công");
    }

    [HttpPost("stop-game")]
    [HasPermission("pet.play")]
    public async Task<IActionResult> StopGame()
    {
        await _petService.StopGame();
        return SuccessResponse("Dừng trò chơi thành công");
    }

    [HttpPost("feed")]
    [HasPermission("pet.feed")]
    public async Task<IActionResult> Feed(int foodId)
    {
        await _petService.Feed(foodId);
        return SuccessResponse("Cho thú cưng ăn thành công");
    }

    [HttpPost("play-active")]
    [HasPermission("pet.play")]
    public async Task<ActionResult> PlayActiveAsync(int id)
    {
        await _petService.PlayActivity(id);
        return SuccessResponse("play successfully");
    }
}