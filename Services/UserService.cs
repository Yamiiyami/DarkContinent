using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using DarkContinent.Helpers;
using DarkContinent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;


public class UserServices
{
    private readonly AuthDbContext _authContext;
    private readonly UserManager<User> _userManager;
    public UserServices(AuthDbContext authContext, UserManager<User> userManager)
    {
        _authContext = authContext;
        _userManager = userManager;

    }

    public async Task<List<User>> GetList(int page = 1, int pageSize = 10)
    {
        return await _authContext.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<User> Login(LoginRequest request)
    {
        var user = await _authContext.Users
        .FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!HashPassword.VerifyPass(request.Password, user.PasswordHash!))
        {
            throw new Exception("Invalid password");
        }
        return user;
    }


    public async Task<User?> Register(RegisterRequest request)
    {

        User? currentUser = await _authContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (currentUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Email,
            Email = request.Email,
            PasswordHash = HashPassword.HashPass(request.Password)
        };

        _authContext.Users.Add(user);
        await _authContext.SaveChangesAsync();
        return user;
    }

}