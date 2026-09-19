
namespace DarkContinent.DTOs.Requests;

public class CreateFoodRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public short HungerGain { get; set; }
    public short EnergyGain { get; set; }
    public short HappinessGain { get; set; }
}

