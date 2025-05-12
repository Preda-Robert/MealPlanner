using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DietaryPreferenceController
{
    private readonly IUnitOfServices _unitOfServices;
    public DietaryPreferenceController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }

    [HttpPost]
    public async Task<ActionResult<DietaryPreferenceDTO>> CreateDietaryPreference(DietaryPreferenceDTO dietaryPreferenceDTO)
    {
        var dietaryPreferenceServiceResult = await _unitOfServices.Service<DietaryPreferences, DietaryPreferenceDTO>().Create(dietaryPreferenceDTO);
        return dietaryPreferenceServiceResult;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<DietaryPreferenceDTO>>> GetDietaryPreferences()
    {
        var dietaryPreferencesServiceResult = await _unitOfServices.Service<DietaryPreferences, DietaryPreferenceDTO>().GetAllAsync();
        return dietaryPreferencesServiceResult;
    }
}
