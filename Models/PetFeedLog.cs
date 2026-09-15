using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class PetFeedLog
{
    public long Id { get; set; }

    public int PetId { get; set; }

    public int FoodId { get; set; }

    public decimal? HungerBefore { get; set; }

    public decimal? HungerAfter { get; set; }

    public decimal? EnergyBefore { get; set; }

    public decimal? EnergyAfter { get; set; }

    public bool WasOverfed { get; set; }

    public DateTime FedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Food Food { get; set; } = null!;

    public virtual Pet Pet { get; set; } = null!;
}
