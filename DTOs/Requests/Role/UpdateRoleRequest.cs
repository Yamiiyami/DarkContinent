namespace DarkContinent.DTOs.Requests;

public class UpdateRoleRequest
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = null!;
    
}