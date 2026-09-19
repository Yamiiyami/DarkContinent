using DarkContinent.Data;
using DarkContinent.DTOs.Responses;
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
                IsAlive = p.IsAlive
            }
        ).ToListAsync();
    }

    

}