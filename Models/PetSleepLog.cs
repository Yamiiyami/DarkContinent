using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class PetSleepLog
{
    public long Id { get; set; }

    public int PetId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public decimal? EnergyBefore { get; set; }

    public decimal? EnergyAfter { get; set; }

    public int DurationInMinutes { get; set; }

    public bool IsFullSleep { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Pet Pet { get; set; } = null!;
}
