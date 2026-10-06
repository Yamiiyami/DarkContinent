using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class Activity : IHasTimestamps, ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte Type { get; set; }

    public int ExpGain { get; set; }

    public short HappinessGain { get; set; }

    public short EnergyCost { get; set; }

    public short HungerCost { get; set; }

    public byte MinEnergyRequired { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<PetActionsLog> PetActionsLogs { get; set; } = new List<PetActionsLog>();
}
