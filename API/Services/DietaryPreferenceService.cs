using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class DietaryPreferenceService : BaseService<DietaryPreferences, DietaryPreferenceDTO>, IDietaryPreferenceService
{
    public DietaryPreferenceService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<ActionResult> SaveDietaryPreference(int userId, SaveDietPreferenceDTO saveDietPreferenceDTO)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new NotFoundObjectResult("User not found");
        }
        
        var existingPreference = await _unitOfWork.DietaryPreferenceRepository.GetDietaryPreferenceByUserId(userId);
        if (existingPreference != null)
        {
            _unitOfWork.DietaryPreferenceRepository.Delete(existingPreference);
            await _unitOfWork.SaveAsync(); 
        }
        
        var dietType = await _unitOfWork.Repository<DietType>().GetByIdAsync(saveDietPreferenceDTO.DietTypeId);
        var servingType = await _unitOfWork.ServingTypeRepository.GetByIdAsync(saveDietPreferenceDTO.ServingTypeId);
        
        if (dietType == null)
        {
            return new NotFoundObjectResult("Diet type not found");
        }
        
        if (servingType == null)
        {
            return new NotFoundObjectResult("Serving type not found");
        }
        
        var dietaryPreference = new DietaryPreferences
        {
            UserId = userId,
            User = user,
            DietTypeId = saveDietPreferenceDTO.DietTypeId,
            DietType = dietType,
            ServingTypeId = saveDietPreferenceDTO.ServingTypeId,
            ServingType = servingType,
            Allergies = []
        };
        
        foreach (var allergyId in saveDietPreferenceDTO.AllergyIds)
        {
            var allergyEntity = await _unitOfWork.AllergyRepository.GetByIdAsync(allergyId);
            if (allergyEntity != null)
            {
                dietaryPreference.Allergies.Add(allergyEntity);
            }
        }
        
        user.HasDoneSetup = true;
 
        await _unitOfWork.DietaryPreferenceRepository.AddAsync(dietaryPreference);
        await _unitOfWork.SaveAsync();
        user.DietaryPreferences = dietaryPreference; 
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();  
    
        return new OkObjectResult(_mapper.Map<DietaryPreferenceDTO>(dietaryPreference));
    }
}