using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class PetType : IHasTimestamps, ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int MaxHunger { get; set; }

    public int MaxEnergy { get; set; }

    public int MaxHappiness { get; set; }

    public decimal TimeToZeroHungerMinutes { get; set; } = 10;

    public decimal TimeToZeroEnergyMinutes { get; set; } = 12;

    public decimal TimeToZeroHappinessMinutes { get; set; } = 16;

    public short SleepEnergyGainPerHour { get; set; }

    public short SleepHungerCostPerHour { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
