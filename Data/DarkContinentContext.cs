using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DarkContinent.Models;

namespace DarkContinent.Data;

public partial class DarkContinentContext : DbContext
{
    public DarkContinentContext()
    {
    }

    public DarkContinentContext(DbContextOptions<DarkContinentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetActionsLog> PetActionsLogs { get; set; }

    public virtual DbSet<PetFeedLog> PetFeedLogs { get; set; }

    public virtual DbSet<PetSleepLog> PetSleepLogs { get; set; }

    public virtual DbSet<PetType> PetTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__activiti__3213E83FB0FDE3B2");

            entity.ToTable("activities");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.EnergyCost).HasColumnName("energy_cost");
            entity.Property(e => e.ExpGain).HasColumnName("exp_gain");
            entity.Property(e => e.HappinessGain).HasColumnName("happiness_gain");
            entity.Property(e => e.HungerCost).HasColumnName("hunger_cost");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MinEnergyRequired).HasColumnName("min_energy_required");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__foods__3213E83FC7DCD13B");

            entity.ToTable("foods");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.EnergyGain).HasColumnName("energy_gain");
            entity.Property(e => e.HappinessGain).HasColumnName("happiness_gain");
            entity.Property(e => e.HungerGain).HasColumnName("hunger_gain");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pets__3213E83F25E8AAEF");

            entity.ToTable("pets");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BornAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("born_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DiedAt)
                .HasColumnType("datetime")
                .HasColumnName("died_at");
            entity.Property(e => e.Energy)
                .HasDefaultValue(100.00m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy");
            entity.Property(e => e.Exp).HasColumnName("exp");
            entity.Property(e => e.Happiness)
                .HasDefaultValue(100.00m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("happiness");
            entity.Property(e => e.Hunger)
                .HasDefaultValue(100.00m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hunger");
            entity.Property(e => e.IsAlive)
                .HasDefaultValue(true)
                .HasColumnName("is_alive");
            entity.Property(e => e.LastCalculatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_calculated_at");
            entity.Property(e => e.Level)
                .HasDefaultValue(1)
                .HasColumnName("level");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Pets)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pets__type__6477ECF3");
        });

        modelBuilder.Entity<PetActionsLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_acti__3213E83F0E2BB702");

            entity.ToTable("pet_actions_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EnergyAfter)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy_after");
            entity.Property(e => e.EnergyBefore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy_before");
            entity.Property(e => e.HappinessAfter)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("happiness_after");
            entity.Property(e => e.HappinessBefore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("happiness_before");
            entity.Property(e => e.HungerAfter)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hunger_after");
            entity.Property(e => e.HungerBefore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hunger_before");
            entity.Property(e => e.PerformedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("performed_at");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Activity).WithMany(p => p.PetActionsLogs)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pet_actio__activ__02FC7413");

            entity.HasOne(d => d.Pet).WithMany(p => p.PetActionsLogs)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pet_actio__pet_i__02084FDA");
        });

        modelBuilder.Entity<PetFeedLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_feed__3213E83FE1CD944A");

            entity.ToTable("pet_feed_logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EnergyAfter)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy_after");
            entity.Property(e => e.EnergyBefore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy_before");
            entity.Property(e => e.FedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fed_at");
            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.HungerAfter)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hunger_after");
            entity.Property(e => e.HungerBefore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hunger_before");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WasOverfed).HasColumnName("was_overfed");

            entity.HasOne(d => d.Food).WithMany(p => p.PetFeedLogs)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pet_feed___food___0A9D95DB");

            entity.HasOne(d => d.Pet).WithMany(p => p.PetFeedLogs)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pet_feed___pet_i__09A971A2");
        });

        modelBuilder.Entity<PetSleepLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_slee__3213E83F61630BF5");

            entity.ToTable("pet_sleep_logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DurationInMinutes).HasColumnName("duration_in_minutes");
            entity.Property(e => e.EndAt)
                .HasColumnType("datetime")
                .HasColumnName("end_at");
            entity.Property(e => e.EnergyAfter)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy_after");
            entity.Property(e => e.EnergyBefore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("energy_before");
            entity.Property(e => e.IsFullSleep).HasColumnName("is_full_sleep");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.StartAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("start_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Pet).WithMany(p => p.PetSleepLogs)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pet_sleep__pet_i__123EB7A3");
        });

        modelBuilder.Entity<PetType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pet_type__3213E83F2CAA62ED");

            entity.ToTable("pet_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsAlive)
                .HasDefaultValue(true)
                .HasColumnName("is_alive");
            entity.Property(e => e.MaxEnergy)
                .HasDefaultValue(100)
                .HasColumnName("max_energy");
            entity.Property(e => e.MaxHappiness)
                .HasDefaultValue(100)
                .HasColumnName("max_happiness");
            entity.Property(e => e.MaxHunger)
                .HasDefaultValue(100)
                .HasColumnName("max_hunger");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.SleepEnergyGainPerHour)
                .HasDefaultValue((short)10)
                .HasColumnName("sleep_energy_gain_per_hour");
            entity.Property(e => e.SleepHungerCostPerHour)
                .HasDefaultValue((short)5)
                .HasColumnName("sleep_hunger_cost_per_hour");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
