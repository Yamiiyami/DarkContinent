namespace DarkContinent.DTOs.Responses;

public class ActivityResponse
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

    public bool IsActive { get; set; }
}
