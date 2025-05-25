using API.DTO;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MealPlansController : BaseAPIController
{
  private readonly IUnitOfServices _unitOfServices;
  public MealPlansController(IUnitOfServices unitOfServices)
  {
    _unitOfServices = unitOfServices;
  }

  [HttpPost]
  public async Task<ActionResult<MealPlanDTO>> CreateMealPlan([FromBody] MealPlanDTO mealPlan)
  {
    var mealPlanServiceResult = await _unitOfServices.MealPlanService.Create(mealPlan);
    return mealPlanServiceResult;
  }

  [HttpGet]
  public async Task<ActionResult<ICollection<MealPlanDTO>>> GetMealPlans([FromQuery] MealPlanParams mealPlanParams)
  {
    mealPlanParams.CurrentUser = User.GetUserId();
    var mealPlanServiceResult = await _unitOfServices.MealPlanService.GetAllAsync(mealPlanParams);
    var mealPlans = mealPlanServiceResult.Value!;
    Response.AddPaginationHeader(mealPlans.CurrentPage, mealPlans);
    return Ok(mealPlans);
  }

  [HttpGet("by-range")]
  public async Task<ActionResult<MealPlanDTO?>> GetMealPlanByDateRange(DateTime startDate, DateTime endDate)
  {
    try
    {
      var userId = User.GetUserId();

      var result = await _unitOfServices.MealPlanService.GetMealPlanByDateRange(startDate, endDate, userId);

      // Return Ok with null value if no meal plan exists
      // This allows the frontend to show "create meal plan" UI
      return Ok(result.Value);
    }
    catch (Exception ex)
    {
      // Return the actual error message for debugging
      return StatusCode(500, $"Error: {ex.Message}");
    }
  }
}