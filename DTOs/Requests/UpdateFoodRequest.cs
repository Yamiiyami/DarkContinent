

using System.ComponentModel.DataAnnotations;

namespace DarkContinent.DTOs.Requests;

public class UpdateFoodRequest
{
    [Required]
    public int Id { get; set; }
    [Required( ErrorMessage = "Name is required.")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "HungerRestore must be a non-negative integer.")]
    public int HungerGain { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "HungerRestore must be a non-negative integer.")]
    public int EnergyGain { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "HungerRestore must be a non-negative integer.")]
    public int HappinessGain { get; set; }
}