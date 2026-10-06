using DarkContinent.Data;
using DarkContinent.DTOs.Requests;
using DarkContinent.DTOs.Responses;
using DarkContinent.Helpers;
using DarkContinent.Models;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;


public class FoodService
{
    private readonly DarkContinentContext _context;

    public FoodService(DarkContinentContext context)
    {
        _context = context;
    }

    public async Task<List<FoodResponse>> GetPageAsync(int page = 1, int pageSize = 10)
    {

        return await _context.Foods.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(f =>
                new FoodResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    HungerGain = f.HungerGain,
                    EnergyGain = f.EnergyGain,
                    HappinessGain = f.HappinessGain
                }
            ).ToListAsync();
    }

    public async Task CreateAsync(CreateFoodRequest food)
    {

        int id = IdGenerator.GenerateRandomId();
        Food newFood = new Food
        {
            Id = id,
            Name = food.Name,
            Description = food.Description,
            HungerGain = food.HungerGain,
            EnergyGain = food.EnergyGain,
            HappinessGain = food.HappinessGain
        };

        _context.Foods.Add(newFood);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateFoodRequest food)
    {
        await _context.Foods
            .Where(f => f.Id == food.Id)
            .ExecuteUpdateAsync(f => f
                .SetProperty(f => f.Name, food.Name)
                .SetProperty(f => f.Description, food.Description)
                .SetProperty(f => f.HungerGain, food.HungerGain)
                .SetProperty(f => f.EnergyGain, food.EnergyGain)
                .SetProperty(f => f.HappinessGain, food.HappinessGain)
            );

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var food = await _context.Foods.FindAsync(id);
        if(food is null)
        {
            throw new Exception("Không tìm thấy món ăn");
        }
        _context.Foods.Remove(food); 
        var result = await _context.SaveChangesAsync(); 
        return result > 0;
    }

}