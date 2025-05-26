using System;
using API.Entities;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ServingTypeRepository : BaseRepository<ServingType>, IServingTypeRepository
{
    public ServingTypeRepository(DataContext context) : base(context)
    {
    }

    public override async Task<ICollection<ServingType>> GetAllAsync()
    {
        return await _context.ServingTypes.Where(st => st.Official).ToListAsync();
    }
}
