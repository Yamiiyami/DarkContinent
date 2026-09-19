namespace DarkContinent.DTOs.Responses
{
    public class FoodResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public short HungerGain { get; set; }
        public short EnergyGain { get; set; }
        public short HappinessGain { get; set; } 
    }

    
}