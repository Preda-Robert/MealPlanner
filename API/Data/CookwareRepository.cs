using System;
using API.Entities;
using API.Interfaces;
using API.Repositories;

namespace API.Data;

public class CookwareRepository : BaseRepository<Cookware>, ICookwareRepository
{
    public CookwareRepository(DataContext context) : base(context)
    {
    }
}
