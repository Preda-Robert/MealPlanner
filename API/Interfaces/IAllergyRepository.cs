using System;
using API.Entities;

namespace API.Interfaces;

public interface IAllergyRepository
{
    void CreateAllergy(Allergy allergy);
    void UpdateAllergy(Allergy allergy);
    void DeleteAllergy(Allergy allergy);
    Allergy GetAllergy(int id);
    ICollection<Allergy> GetAllAllergies();
}
