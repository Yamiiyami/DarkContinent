using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using DarkContinent.DTOs.Responses;
using DarkContinent.Helpers;
using DarkContinent.Models;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;


public class PetTypeService
{
    private readonly DarkContinentContext _context;

    public PetTypeService(DarkContinentContext context)
    {
        _context = context;
    }

    public async Task<List<PetTypeResponse>> GetPageAsync(int page = 1, int pageSize = 10)
    {
        return await _context.PetTypes.Skip((page - 1) * pageSize).Take(pageSize).Select(
            p => new PetTypeResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsAlive = p.IsAlive,
                MaxHunger = p.MaxHunger,
                MaxEnergy = p.MaxEnergy,
                MaxHappiness = p.MaxHappiness,
                SleepEnergyGainPerHour = p.SleepEnergyGainPerHour,
                SleepHungerCostPerHour = p.SleepHungerCostPerHour
            }
        ).ToListAsync();
    }

    public async Task<PetTypeResponse?> CreateAsync(CreatePetTypeRequest request)
    {
        var newPetType = new PetType
        {
            Id = IdGenerator.GenerateRandomId(),
            Name = request.Name,
            Description = request.Description,
            MaxHunger = request.MaxHunger,
            MaxEnergy = request.MaxEnergy,
            MaxHappiness = request.MaxHappiness,
            SleepEnergyGainPerHour = request.SleepEnergyGainPerHour,
            SleepHungerCostPerHour = request.SleepHungerCostPerHour,
            IsAlive = request.IsAlive
        };

        _context.PetTypes.Add(newPetType);
        int result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            return new PetTypeResponse
            {
                Id = newPetType.Id,
                Name = newPetType.Name,
                Description = newPetType.Description,
                MaxHunger = newPetType.MaxHunger,
                MaxEnergy = newPetType.MaxEnergy,
                MaxHappiness = newPetType.MaxHappiness,
                SleepEnergyGainPerHour = newPetType.SleepEnergyGainPerHour,
                SleepHungerCostPerHour = newPetType.SleepHungerCostPerHour,
                IsAlive = newPetType.IsAlive
            };
        }
        return null;
    }

    public async Task<bool> UpdateAsync(UpdatePetTypeRequest petType)
    {
        int result = await _context.PetTypes
            .Where(p => p.Id == petType.Id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(p => p.Name, petType.Name)
                .SetProperty(p => p.Description, petType.Description)
                .SetProperty(p => p.MaxHunger, petType.MaxHunger)
                .SetProperty(p => p.MaxEnergy, petType.MaxEnergy)
                .SetProperty(p => p.MaxHappiness, petType.MaxHappiness)
                .SetProperty(p => p.SleepEnergyGainPerHour, petType.SleepEnergyGainPerHour)
                .SetProperty(p => p.SleepHungerCostPerHour, petType.SleepHungerCostPerHour)
                .SetProperty(p => p.IsAlive, petType.IsAlive)
            );
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _context.PetTypes.Where(p => p.Id == id).ExecuteDeleteAsync() != 0;
    }

}