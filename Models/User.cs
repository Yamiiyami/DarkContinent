using Microsoft.AspNetCore.Identity;

namespace DarkContinent.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }

}