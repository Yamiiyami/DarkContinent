using System;
using System.Collections.Generic;

namespace DarkContinent.Models;

public partial class Pet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int Age { get; set; }

    public decimal Hunger { get; set; }

    public decimal Energy { get; set; }

    public decimal Happiness { get; set; }

    public int Level { get; set; }

    public int Exp { get; set; }

    public DateTime BornAt { get; set; }

    public DateTime? DiedAt { get; set; }

    public bool IsAlive { get; set; }

    public DateTime LastCalculatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<PetActionsLog> PetActionsLogs { get; set; } = new List<PetActionsLog>();

    public virtual ICollection<PetFeedLog> PetFeedLogs { get; set; } = new List<PetFeedLog>();

    public virtual ICollection<PetSleepLog> PetSleepLogs { get; set; } = new List<PetSleepLog>();

    public virtual PetType TypeNavigation { get; set; } = null!;
}
