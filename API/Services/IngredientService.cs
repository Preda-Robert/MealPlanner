using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class IngredientService : BaseService<Ingredient, IngredientDTO>, IIngredientService
{
    public IngredientService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public override async Task<ActionResult<IngredientDTO>> Create(IngredientDTO dto)
    {
        string logPath = "logs/ingredient_seeding.log";
        Directory.CreateDirectory("logs");

        void Log(string message)
        {
            File.AppendAllText(logPath, $"[{DateTime.Now}] {message}{Environment.NewLine}");
        }

        try
        {
            Log($"Attempting to create ingredient: {dto.Name}");

            var existingIngredient = await _unitOfWork.IngredientRepository.GetIngredientByNameAsync(dto.Name);
            if (existingIngredient != null)
            {
                Log($"Ingredient '{dto.Name}' already exists. Skipping.");
                return _mapper.Map<IngredientDTO>(existingIngredient);
            }

            var category = await _unitOfWork.IngredientCategoryRepository.GetCategoryByNameAsync(dto.Category.Name);
            if (category == null)
            {
                Log($"Failed to create '{dto.Name}': Category '{dto.Category.Name}' not found.");
                return new BadRequestObjectResult($"Category with name {dto.Category.Name} does not exist.");
            }

            Allergy? allergy = null;
            if (dto.Allergy != null)
            {
                allergy = await _unitOfWork.AllergyRepository.GetAllergyByNameAsync(dto.Allergy.Name);
                if (allergy == null)
                {
                    Log($"Failed to create '{dto.Name}': Allergy '{dto.Allergy.Name}' not found.");
                    return new BadRequestObjectResult($"Allergy with name {dto.Allergy.Name} does not exist.");
                }
            }

            var createdIngredient = _mapper.Map<Ingredient>(dto);
            createdIngredient.Category = category;
            createdIngredient.Allergy = allergy;

            category.Ingredients.Add(createdIngredient);

            await _unitOfWork.IngredientRepository.AddAsync(createdIngredient);
            await _unitOfWork.SaveAsync();

            Log($"Successfully created ingredient: {dto.Name}");

            return _mapper.Map<IngredientDTO>(createdIngredient);
        }
        catch (Exception ex)
        {
            Log($"Exception while creating ingredient '{dto.Name}': {ex.Message}");
            throw; // optional: rethrow or handle as needed
        }
    }
}
