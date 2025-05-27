using API.DTO;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IMealPlanService : IBaseService<MealPlan, MealPlanDTO>
{
  Task<ActionResult<PagedList<MealPlanDTO>>> GetAllAsync(MealPlanParams mealPlanParams);
  Task<ActionResult<MealPlanDTO?>> GetMealPlanByDateRange(DateTime startDate, DateTime endDate, int userId);
  Task<ActionResult<bool>> DeleteAsync(int id);
  Task<ActionResult<MealPlanDTO>> UpdateAsync(int id, MealPlanDTO mealPlanDTO);
}
