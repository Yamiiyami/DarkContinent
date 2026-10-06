using DarkContinent.Data;
using DarkContinent.Models;
using DarkContinent.Requests;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;

public class PetService
{
    private readonly DarkContinentContext _db;
    public PetService(DarkContinentContext db)
    {
        _db = db;
    }

    public async Task CreatePet(CreatePetRequest request)
    {
        var petType = await _db.PetTypes.FirstOrDefaultAsync(p => p.Id == request.Type);
        if (petType is null)
        { 
            throw new Exception("Loại thú cưng không tồn tại");
        }

        var IsExists = await _db.Pets.AnyAsync(p => p.IsAlive == true);
        if (IsExists)
        {
            throw new Exception("Bạn chỉ có thể nuôi 1 thú cưng");
        }

        var pet = new Pet
        {
            Name = request.Name,
            Type = request.Type,
            Happiness = petType.MaxHappiness,
            Energy = petType.MaxEnergy,
            Hunger = petType.MaxHunger,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Age = 0,
            IsAlive = true,
            BornAt = DateTime.UtcNow,
            Exp = 0,
            Level = 1,

        };

        _db.Pets.Add(pet);
        await _db.SaveChangesAsync();
    }

}