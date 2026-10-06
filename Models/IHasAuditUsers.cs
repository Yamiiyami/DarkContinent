namespace DarkContinent.Models;

public interface IHasAuditUsers
{
    string? CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}