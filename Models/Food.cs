using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class Food
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short HungerGain { get; set; }

    public short EnergyGain { get; set; }

    public short HappinessGain { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<PetFeedLog> PetFeedLogs { get; set; } = new List<PetFeedLog>();
}
