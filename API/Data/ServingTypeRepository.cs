using System;
using API.Entities;
using API.Interfaces;
using API.Repositories;

namespace API.Data;

public class ServingTypeRepository : BaseRepository<ServingType>, IServingTypeRepository
{
    public ServingTypeRepository(DataContext context) : base(context)
    {
    }
}
