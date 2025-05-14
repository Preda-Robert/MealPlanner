using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AllergyRepository : BaseRepository<Allergy>,IAllergyRepository
{
    public AllergyRepository(DataContext context) : base(context)
    {
    }

    public async Task<bool> AllergyExistsAsync(string name)
    {
        return await _context.Allergies.AnyAsync(a => a.Name == name);
    }

    public Task<bool> AllergyExistsAsync(int id)
    {
        return _context.Allergies.AnyAsync(a => a.Id == id);
    }

    public async Task<ICollection<Allergy>> GetAllAllergiesAsync()
    {
        return await _context.Allergies.ToListAsync();
    }

    public async Task<ICollection<Allergy>> GetAllAllergiesByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<Allergy>> GetAllergiesAsync(AllergyParams allergyParams)
    {
        var query = _context.Allergies.AsQueryable();

        if (!string.IsNullOrEmpty(allergyParams.SearchTerm))
        {
            query = query.Where(a => a.Name.ToLower().Contains(allergyParams.SearchTerm.ToLower()));
        }

        return PagedList<Allergy>.CreateAsync(query, allergyParams.PageNumber, allergyParams.PageSize);
    }

    public async Task<Allergy?> GetAllergyByNameAsync(string name)
    {
        return await _context.Allergies
            .FirstOrDefaultAsync(a => a.Name == name);
    }
}
