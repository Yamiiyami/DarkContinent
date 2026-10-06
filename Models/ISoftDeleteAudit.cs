namespace DarkContinent.Models;
public interface ISoftDeleteAudit
{
    string? DeletedBy { get; set; }
}