using DarkContinent.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarkContinent.Data;

public class AuthDbContext : IdentityDbContext<User>
{
    CurrentUserService? _currentUserService;

    public AuthDbContext(DbContextOptions<AuthDbContext> options, CurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            string? tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
            
            Type type = typeof(ISoftDelete);
            if (type.IsAssignableFrom(entityType.ClrType))
            {
                // builder.Entity(entityType.ClrType)
                //    .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
            }

        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries();

        foreach (EntityEntry entry in entries)
        {
            if (entry.Entity is IHasTimestamps hasTimestamps)
            {
                if (entry.State == EntityState.Added)
                {
                    hasTimestamps.CreatedAt = DateTime.UtcNow;
                    hasTimestamps.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    hasTimestamps.UpdatedAt = DateTime.UtcNow;
                }
            }

            if(entry.Entity is IHasAuditUsers hasAuditUsers)
            {
                if(entry.State == EntityState.Added)
                {
                    hasAuditUsers.CreatedBy = _currentUserService?.UserId;
          
                } else if(entry.State == EntityState.Modified)
                {
                    hasAuditUsers.UpdatedBy = _currentUserService?.UserId;
                }
            }

            if (entry.Entity is ISoftDelete softDelete && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                softDelete.DeletedAt = DateTime.UtcNow;
            }

        }
        return base.SaveChangesAsync(cancellationToken);
    }



}

