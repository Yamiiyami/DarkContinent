using DarkContinent.Data;
using DarkContinent.Helpers;
using DarkContinent.Models;
using DarkContinent.Requests;
using Microsoft.EntityFrameworkCore;

namespace DarkContinent.Services;

public class PetService
{
    private readonly DarkContinentContext _context;

    public PetService(DarkContinentContext db)
    {
        _context = db;
    }

    public async Task CreatePet(CreatePetRequest request, CancellationToken ct = default)
    {
        var petType = await _context.PetTypes.FirstOrDefaultAsync(p => p.Id == request.Type, ct)
            ?? throw new InvalidOperationException("Loại thú cưng không tồn tại");

        if (await _context.Pets.AnyAsync(p => p.IsAlive, ct))
            throw new InvalidOperationException("Bạn chỉ có thể nuôi 1 thú cưng");

        var now = DateTime.UtcNow;
        _context.Pets.Add(new Pet
        {
            Id = IdGenerator.GenerateRandomId(),
            Name = request.Name,
            Type = request.Type,
            Hunger = petType.MaxHunger,
            Energy = petType.MaxEnergy,
            Happiness = petType.MaxHappiness,
            LastCalculatedAt = now,
            Age = 0,
            IsAlive = true,
            Exp = 0,
            Level = 1,
            BornAt = DateTime.UtcNow,
        });

        var result = await _context.SaveChangesAsync(ct);
    }

    public Task<Pet?> GetCurrentPet()
    {
        return _context.Pets
            .Include(p => p.TypeNavigation)
            .Select(p => new Pet
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                Hunger = p.Hunger,
                Energy = p.Energy,
                Happiness = p.Happiness,
                Age = p.Age,
                IsAlive = p.IsAlive,
                Exp = p.Exp,
                Level = p.Level,
                IsPlaying = p.IsPlaying,
                LastCalculatedAt = p.LastCalculatedAt,
                TypeNavigation = new PetType
                {
                    Id = p.TypeNavigation.Id,
                    Name = p.TypeNavigation.Name,
                    Description = p.TypeNavigation.Description,
                    MaxHunger = p.TypeNavigation.MaxHunger,
                    MaxEnergy = p.TypeNavigation.MaxEnergy,
                    MaxHappiness = p.TypeNavigation.MaxHappiness,
                    SleepEnergyGainPerHour = p.TypeNavigation.SleepEnergyGainPerHour,
                    SleepHungerCostPerHour = p.TypeNavigation.SleepHungerCostPerHour
                }
            })
            .FirstOrDefaultAsync(p => p.IsAlive);
    }

    private Task<Pet?> GetAlivePetRaw(CancellationToken ct = default) =>
        _context.Pets
            .Include(p => p.TypeNavigation)
            .FirstOrDefaultAsync(p => p.IsAlive, ct);


    public async Task<Pet> LoadPetWithDecayAsync(CancellationToken ct = default)
    {
        var pet = await GetAlivePetRaw(ct) ?? throw new KeyNotFoundException("Bạn chưa có thú cưng nào");

        if (!pet.IsPlaying) return pet;

        var now = DateTime.UtcNow;
        var minutes = (decimal)(now - pet.LastCalculatedAt).TotalMinutes;
        if (minutes <= 0) return pet;

        var t = pet.TypeNavigation;
        pet.Hunger = Math.Max(0, pet.Hunger - Decay(t.MaxHunger, t.TimeToZeroHungerMinutes, minutes));
        pet.Energy = Math.Max(0, pet.Energy - Decay(t.MaxEnergy, t.TimeToZeroEnergyMinutes, minutes));
        pet.Happiness = Math.Max(0, pet.Happiness - Decay(t.MaxHappiness, t.TimeToZeroHappinessMinutes, minutes));
        pet.LastCalculatedAt = now;

        if (pet.Hunger <= 0)
        {
            pet.IsAlive = false;
            pet.IsPlaying = false;
            pet.DiedAt = now;
            await _context.SaveChangesAsync(ct);
            throw new InvalidOperationException("Thú cưng của bạn đã chết vì không được chăm sóc đúng cách.");
        }

        await _context.SaveChangesAsync(ct);
        return pet;
    }

    private static decimal Decay(decimal max, decimal minutesToZero, decimal minutesPassed)
        => minutesToZero <= 0 ? 0 : max / minutesToZero * minutesPassed;

    public async Task StartGame(CancellationToken ct = default)
    {
        var pet = await GetAlivePetRaw(ct)
            ?? throw new KeyNotFoundException("Bạn chưa có thú cưng nào");

        if (pet.IsPlaying)
            throw new InvalidOperationException("Trò chơi đã được bắt đầu");

        pet.IsPlaying = true;
        pet.LastCalculatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }

    public async Task StopGame(CancellationToken ct = default)
    {
        var pet = await LoadPetWithDecayAsync(ct);

        pet.IsPlaying = false;
        await _context.SaveChangesAsync(ct);
    }

    public async Task PlayActivity(int activityId, CancellationToken ct = default)
    {
        var pet = await LoadPetWithDecayAsync(ct);

        if (!pet.IsPlaying)
            throw new InvalidOperationException("Hãy bắt đầu trò chơi trước");

        if (pet.IsSleep)
            throw new InvalidOperationException("Thú cưng đang ngủ");

        var activity = await _context.Activities.FindAsync(activityId, ct)
            ?? throw new KeyNotFoundException("Activity not found");

        if (pet.Energy < activity.MinEnergyRequired)
            throw new InvalidOperationException("Không đủ năng lượng để thực hiện hoạt động này");
        if (pet.Energy < activity.EnergyCost)
            throw new InvalidOperationException("Không đủ năng lượng để thực hiện hoạt động này");
        if (pet.Hunger <= activity.HungerCost)
            throw new InvalidOperationException("Thú cưng quá đói để thực hiện hoạt động này");

        var t = pet.TypeNavigation;
        var log = new PetActionsLog
        {
            PetId = pet.Id,
            ActivityId = activityId,
            PerformedAt = DateTime.UtcNow,
            HungerBefore = pet.Hunger,
            EnergyBefore = pet.Energy,
            HappinessBefore = pet.Happiness
        };

        pet.Energy = Math.Clamp(pet.Energy - activity.EnergyCost, 0, t.MaxEnergy);
        pet.Hunger = Math.Clamp(pet.Hunger - activity.HungerCost, 0, t.MaxHunger);
        pet.Happiness = Math.Clamp(pet.Happiness + activity.HappinessGain, 0, t.MaxHappiness);
        pet.Exp += activity.ExpGain;

        log.HungerAfter = pet.Hunger;
        log.EnergyAfter = pet.Energy;
        log.HappinessAfter = pet.Happiness;

        _context.PetActionsLogs.Add(log);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Feed(int foodId, CancellationToken ct = default)
    {
        var pet = await LoadPetWithDecayAsync(ct);
        if (!pet.IsPlaying)
            throw new InvalidOperationException("Hãy bắt đầu trò chơi trước");

        if (pet.IsSleep)
            throw new InvalidOperationException("Thú cưng đang ngủ");

        var food = await _context.Foods.FindAsync(foodId, ct)
            ?? throw new KeyNotFoundException("Food not found");

        var type = pet.TypeNavigation;
        if (pet.Hunger >= type.MaxHunger && pet.Energy >= type.MaxEnergy && pet.Happiness >= type.MaxHappiness)
            throw new InvalidOperationException("Thú cưng của bạn đã no và hạnh phúc, không cần ăn thêm");

        pet.Hunger = Math.Clamp(pet.Hunger + food.HungerGain, 0, type.MaxHunger);
        pet.Energy = Math.Clamp(pet.Energy + food.EnergyGain, 0, type.MaxEnergy);
        pet.Happiness = Math.Clamp(pet.Happiness + food.HappinessGain, 0, type.MaxHappiness);

        await _context.SaveChangesAsync(ct);
    }

    public async Task Sleep(CancellationToken ct = default)
    {
        Pet pet = await LoadPetWithDecayAsync(ct);

        if (!pet.IsPlaying)
            throw new InvalidOperationException("Hãy bắt đầu trò chơi trước");

        if (pet.IsSleep)
            throw new InvalidOperationException("Thú cưng đang ngủ");

        var type = pet.TypeNavigation;
        var now = DateTime.UtcNow;

        pet.IsSleep = true;
        pet.SleepStartedAt = now;
        await _context.SaveChangesAsync(ct);
    }

    public async Task WakeUp(CancellationToken ct = default)
    {
        var pet = await LoadPetWithDecayAsync(ct);
        if (!pet.IsPlaying)
            throw new InvalidOperationException("Hãy bắt đầu trò chơi trước");

        if (!pet.IsSleep || pet.SleepStartedAt == null)
            throw new InvalidOperationException("Thú cưng của bạn đang thức");

        var type = pet.TypeNavigation;
        var now = DateTime.UtcNow;

        var sleepDurationHours = (now - pet.SleepStartedAt.Value).TotalHours;

        pet.Energy = Math.Clamp(pet.Energy + (decimal)(type.SleepEnergyGainPerHour * sleepDurationHours), 0, type.MaxEnergy);
        pet.Hunger = Math.Clamp(pet.Hunger - (decimal)(type.SleepHungerCostPerHour * sleepDurationHours), 0, type.MaxHunger);

        pet.IsSleep = false;
        pet.SleepStartedAt = null;
        pet.LastCalculatedAt = now;

        if (pet.Hunger <= 0)
        {
            pet.IsAlive = false;
            pet.IsPlaying = false;
            pet.DiedAt = now;
            await _context.SaveChangesAsync(ct);
            throw new InvalidOperationException("Thú cưng của bạn đã chết vì đói khi đang ngủ.");
        }
        await _context.SaveChangesAsync(ct);
    }

}