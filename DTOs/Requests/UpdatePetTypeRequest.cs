namespace DarkContinent.DTOs.Requests ;

public class UpdatePetTypeRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int MaxHunger { get; set; }

    public int MaxEnergy { get; set; }

    public int MaxHappiness { get; set; }

    public short SleepEnergyGainPerHour { get; set; }

    public short SleepHungerCostPerHour { get; set; }

    public bool IsAlive { get; set; }

} 