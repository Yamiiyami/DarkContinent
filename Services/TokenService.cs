using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DarkContinent.Data;
using DarkContinent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public record AuthResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpires);

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly AuthDbContext _db;
    private readonly UserManager<User> _userManager;

    public TokenService(IConfiguration config, AuthDbContext db, UserManager<User> userManager)
    {
        _config = config;
        _db = db;
        _userManager = userManager;
    }

    public async Task<AuthResult> IssueTokensAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, expires) = GenerateAccessToken(user, roles);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);
        return new AuthResult(accessToken, refreshToken, expires);
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken)
    {
        var hash = Hash(refreshToken);
        var stored = await _db.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == hash);

        if (stored is null) throw new Exception("Invalid refresh token");


        if (stored.RevokedAt is not null)
        {
            await RevokeAllAsync(stored.UserId);
            throw new Exception("Refresh token has been revoked");
        }

        if (stored.ExpiresAt <= DateTime.UtcNow) throw new Exception("Refresh token has expired");


        var newRaw = GenerateRawToken();
        var newHash = Hash(newRaw);

        stored.RevokedAt = DateTime.UtcNow;
        stored.ReplacedByTokenHash = newHash;

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = stored.UserId,
            TokenHash = newHash,
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshDays())
        });
        await _db.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(stored.User);
        var (accessToken, expires) = GenerateAccessToken(stored.User, roles);
        return new AuthResult(accessToken, newRaw, expires);
    }

    public async Task<bool> RevokeAsync(string refreshToken)
    {
        var hash = Hash(refreshToken);
        var stored = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash);
        if (stored is null || stored.RevokedAt is not null) return false;

        stored.RevokedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task RevokeAllAsync(string userId)
    {
        await _db.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, DateTime.UtcNow));
    }

    private (string token, DateTime expires) GenerateAccessToken(User user, IList<string> roles)
    {
        var jwt = _config.GetSection("Jwt");
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwt["AccessTokenMinutes"] ?? "15"));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new("name", user.UserName!)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwt["Issuer"],
            Audience = jwt["Audience"],
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
                SecurityAlgorithms.HmacSha256)
        };

        return (new JsonWebTokenHandler().CreateToken(descriptor), expires);
    }

    private async Task<string> CreateRefreshTokenAsync(string userId)
    {
        var raw = GenerateRawToken();
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            TokenHash = Hash(raw),
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshDays())
        });
        await _db.SaveChangesAsync();
        return raw;
    }

    private int RefreshDays() => int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7");

    private static string GenerateRawToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private static string Hash(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));


}
