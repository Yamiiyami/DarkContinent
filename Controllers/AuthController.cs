using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using DarkContinent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly TokenService _tokenService;
    private readonly AuthDbContext _authContext;
    private readonly UserServices _userService;
    public AuthController(TokenService tokenService, AuthDbContext authContext, UserServices userService)
    {
        _tokenService = tokenService;
        _authContext = authContext;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.Login(request);
        var (accessToken, refreshToken, expires) = await _tokenService.IssueTokensAsync(user);

        return SuccessResponse("Login successful", new { accessToken, refreshToken, expires }); 
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        await _userService.Register(request);
        return SuccessResponse("User registered successfully");
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var (accessToken, refreshToken, exp) = await _tokenService.RefreshAsync(request.RefreshToken);
        return SuccessResponse("Token refreshed successfully", new { accessToken, refreshToken, exp });

    }
   
}
