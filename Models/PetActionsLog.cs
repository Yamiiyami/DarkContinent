using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class PetActionsLog
{
    public long Id { get; set; }

    public int PetId { get; set; }

    public int ActivityId { get; set; }

    public DateTime PerformedAt { get; set; }

    public decimal? HungerBefore { get; set; }

    public decimal? HungerAfter { get; set; }

    public decimal? EnergyBefore { get; set; }

    public decimal? EnergyAfter { get; set; }

    public decimal? HappinessBefore { get; set; }

    public decimal? HappinessAfter { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual Pet Pet { get; set; } = null!;
}
