using DarkContinent.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Data;

public class AuthDbContext : IdentityDbContext<User>
{
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach(var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if(tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
       
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


}

