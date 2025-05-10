using System;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IAllergyService : IBaseService<Allergy, AllergyDTO>
{

}
