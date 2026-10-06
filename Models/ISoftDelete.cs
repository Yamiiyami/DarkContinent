namespace DarkContinent.Models;

public interface ISoftDelete
{
    DateTime? DeletedAt { get; set; }
}