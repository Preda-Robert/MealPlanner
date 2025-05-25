using API.DTO;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class MealPlanService : BaseService<MealPlan, MealPlanDTO>, IMealPlanService
{

  public MealPlanService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
  {
  }

  public override async Task<ActionResult<MealPlanDTO>> Create(MealPlanDTO mealPlanDTO)
  {
    try
    {
      // Validate date range
      if (mealPlanDTO.StartDate >= mealPlanDTO.EndDate)
      {
        return new BadRequestObjectResult("Start date must be before end date");
      }

      // Check if meal plan already exists for this date range and user
      var existingMealPlan = await _unitOfWork.MealPlanRepository
        .GetMealPlanByDateRange(mealPlanDTO.StartDate, mealPlanDTO.EndDate, mealPlanDTO.UserId);

      if (existingMealPlan != null)
      {
        return new BadRequestObjectResult("A meal plan already exists for this date range");
      }

      // Map DTO to entity
      var mealPlan = _mapper.Map<MealPlan>(mealPlanDTO);


      // Add the meal plan
      await _unitOfWork.MealPlanRepository.AddAsync(mealPlan);

      // Save changes
      var result = await _unitOfWork.SaveAsync();
      if (!result)
      {
        return new BadRequestObjectResult("Failed to create meal plan");
      }

      // Return the created meal plan with all related data
      var createdMealPlan = await _unitOfWork.MealPlanRepository
        .GetMealPlanByDateRange(mealPlan.StartDate, mealPlan.EndDate, mealPlan.UserId);

      return _mapper.Map<MealPlanDTO>(createdMealPlan);
    }
    catch (Exception ex)
    {
      return new BadRequestObjectResult($"Error creating meal plan: {ex.Message}");
    }
  }

  public async Task<ActionResult<MealPlanDTO>> UpdateAsync(int id, MealPlanDTO mealPlanDTO)
  {
    try
    {
      var existingMealPlan = await _unitOfWork.MealPlanRepository.GetByIdAsync(id);
      if (existingMealPlan == null)
      {
        return new BadRequestObjectResult("Meal plan not found");
      }

      // Validate date range
      if (mealPlanDTO.StartDate >= mealPlanDTO.EndDate)
      {
        return new BadRequestObjectResult("Start date must be before end date");
      }

      // Check if another meal plan exists for this date range (excluding current one)
      var conflictingMealPlan = await _unitOfWork.MealPlanRepository
        .GetMealPlanByDateRange(mealPlanDTO.StartDate, mealPlanDTO.EndDate, mealPlanDTO.UserId);

      if (conflictingMealPlan != null && conflictingMealPlan.Id != id)
      {
        return new BadRequestObjectResult("Another meal plan already exists for this date range");
      }

      // Update basic properties
      existingMealPlan.Name = mealPlanDTO.Name;
      existingMealPlan.Description = mealPlanDTO.Description;
      existingMealPlan.StartDate = mealPlanDTO.StartDate;
      existingMealPlan.EndDate = mealPlanDTO.EndDate;

      // Update meal plan recipes if provided
      if (mealPlanDTO.MealPlanRecipes != null && mealPlanDTO.MealPlanRecipes.Any())
      {
        // Clear existing recipes
        existingMealPlan.MealPlanRecipes.Clear();

        // Add new recipes
        foreach (var recipeDTO in mealPlanDTO.MealPlanRecipes)
        {
          var mealPlanRecipe = new MealPlanRecipe
          {
            MealPlanId = id,
            RecipeId = recipeDTO.Id,
            ServingType = _mapper.Map<ServingType>(recipeDTO.ServingType),
          };
          existingMealPlan.MealPlanRecipes.Add(mealPlanRecipe);
        }
      }

      _unitOfWork.MealPlanRepository.Update(existingMealPlan);

      var result = await _unitOfWork.SaveAsync();
      if (!result)
      {
        return new BadRequestObjectResult("Failed to update meal plan");
      }

      // Return the updated meal plan with all related data
      var updatedMealPlan = await _unitOfWork.MealPlanRepository
        .GetMealPlanByDateRange(existingMealPlan.StartDate, existingMealPlan.EndDate, existingMealPlan.UserId);

      return _mapper.Map<MealPlanDTO>(updatedMealPlan);
    }
    catch (Exception ex)
    {
      return new BadRequestObjectResult($"Error updating meal plan: {ex.Message}");
    }
  }

  public async Task<ActionResult<bool>> DeleteAsync(int id)
  {
    try
    {
      var mealPlan = await _unitOfWork.MealPlanRepository.GetByIdAsync(id);
      if (mealPlan == null)
      {
        return new BadRequestObjectResult("Meal plan not found");
      }

      _unitOfWork.MealPlanRepository.Delete(mealPlan);

      var result = await _unitOfWork.SaveAsync();
      if (!result)
      {
        return new BadRequestObjectResult("Failed to delete meal plan");
      }

      return true;
    }
    catch (Exception ex)
    {
      return new BadRequestObjectResult($"Error deleting meal plan: {ex.Message}");
    }
  }

  public async Task<ActionResult<PagedList<MealPlanDTO>>> GetAllAsync(MealPlanParams mealPlanParams)
  {
    var mealPlansQuery = _unitOfWork.MealPlanRepository.GetMealPlans(mealPlanParams);
    return await PagedList<MealPlanDTO>.CreateAsync(mealPlansQuery.ProjectTo<MealPlanDTO>(_mapper.ConfigurationProvider), mealPlanParams.PageNumber, mealPlanParams.PageSize);
  }

  public async Task<ActionResult<MealPlanDTO?>> GetMealPlanByDateRange(DateTime startDate, DateTime endDate, int userId)
  {
    var mealPlan = await _unitOfWork.MealPlanRepository.GetMealPlanByDateRange(startDate, endDate, userId);

    if (mealPlan == null)
    {
      // Log for debugging purposes
      Console.WriteLine($"No meal plan found for user {userId} between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}");

      // Return a successful response with null value instead of throwing an error
      // This allows the frontend to handle the "no meal plan" case gracefully
      return new ActionResult<MealPlanDTO?>(value: null);
    }

    return _mapper.Map<MealPlanDTO>(mealPlan);
  }
}