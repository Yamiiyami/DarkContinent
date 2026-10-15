using DarkContinent.Data;
using DarkContinent.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using DarkContinent.DTOs.Requests;
using DarkContinent.Helpers;
using DarkContinent.Models;
using DarkContinent.DTOs.Request;

namespace DarkContinent.Services;

public class ActivityService
{
    private readonly DarkContinentContext _context;

    public ActivityService(DarkContinentContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityResponse>> GetPageAsync(int page = 1, int pageSize = 10)
    {
        return await _context.Activities.Skip((page - 1) * pageSize).Take(pageSize).Select(
            a => new ActivityResponse
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Type = a.Type,
                ExpGain = a.ExpGain,
                HappinessGain = a.HappinessGain,
                EnergyCost = a.EnergyCost,
                HungerCost = a.HungerCost,
                MinEnergyRequired = a.MinEnergyRequired,
                IsActive = a.IsActive
            }
        ).ToListAsync();
    }
    public async Task<ActivityResponse?> Create(CreateActivityRequest activity)
    {

        var newActivity = new Activity
        {
            Id = IdGenerator.GenerateRandomId(),
            Name = activity.Name,
            Description = activity.Description,
            Type = activity.Type,
            ExpGain = activity.ExpGain,
            HappinessGain = activity.HappinessGain,
            EnergyCost = activity.EnergyCost,
            HungerCost = activity.HungerCost,
            MinEnergyRequired = activity.MinEnergyRequired,
            IsActive = activity.IsActive
        };

        _context.Activities.Add(newActivity);
        int result = await _context.SaveChangesAsync();
        if (result > 0)
        {

            return new ActivityResponse
            {
                Id = newActivity.Id,
                Name = activity.Name,
                Description = activity.Description,
                Type = activity.Type,
                ExpGain = activity.ExpGain,
                HappinessGain = activity.HappinessGain,
                EnergyCost = activity.EnergyCost,
                HungerCost = activity.HungerCost,
                MinEnergyRequired = activity.MinEnergyRequired,
                IsActive = activity.IsActive
            };
        }
        return null;
    }

    public async Task<ActivityResponse?> updateAsync(UpdateActivityRequest activity)
    {

        int result = await _context.Activities.Where(a => a.Id == activity.Id).ExecuteUpdateAsync(a => a
           .SetProperty(a => a.Name, activity.Name)
           .SetProperty(a => a.Description, activity.Description)
           .SetProperty(a => a.Type, activity.Type)
           .SetProperty(a => a.ExpGain, activity.ExpGain)
           .SetProperty(a => a.HappinessGain, activity.HappinessGain)
           .SetProperty(a => a.EnergyCost, activity.EnergyCost)
           .SetProperty(a => a.HungerCost, activity.HungerCost)
           .SetProperty(a => a.MinEnergyRequired, activity.MinEnergyRequired)
           .SetProperty(a => a.IsActive, activity.IsActive)
       );

        if (result > 0)
        {
            return new ActivityResponse
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                Type = activity.Type,
                ExpGain = activity.ExpGain,
                HappinessGain = activity.HappinessGain,
                EnergyCost = activity.EnergyCost,
                HungerCost = activity.HungerCost,
                MinEnergyRequired = activity.MinEnergyRequired,
                IsActive = activity.IsActive
            };
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _context.Activities.Where(a => a.Id == id).ExecuteDeleteAsync() != 0;
    }

    

}