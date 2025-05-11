using System;
using API.Entities;
using API.Interfaces;
using API.Repositories;

namespace API.Data;

public class AllergyRepository : BaseRepository<Allergy>,IAllergyRepository
{
    public AllergyRepository(DataContext context) : base(context)
    {
    }
}
