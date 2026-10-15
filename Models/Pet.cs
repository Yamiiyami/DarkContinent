using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class Pet : IHasTimestamps, ISoftDelete
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int Age { get; set; } = 0;

    public decimal Hunger { get; set; }

    public decimal Energy { get; set; }

    public decimal Happiness { get; set; }

    public int Level { get; set; } = 1;

    public int Exp { get; set; } = 0;

    public DateTime BornAt { get; set; } = DateTime.UtcNow;

    public DateTime? DiedAt { get; set; }

    public bool IsAlive { get; set; } = true;

    public bool IsPlaying { get; set; } = false;

    public bool IsSleep { get; set; } = true;

    public DateTime? SleepStartedAt { get; set; }

    public DateTime LastCalculatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<PetActionsLog> PetActionsLogs { get; set; } = new List<PetActionsLog>();

    public virtual ICollection<PetFeedLog> PetFeedLogs { get; set; } = new List<PetFeedLog>();

    public virtual ICollection<PetSleepLog> PetSleepLogs { get; set; } = new List<PetSleepLog>();

    public virtual PetType TypeNavigation { get; set; } = null!;
}
