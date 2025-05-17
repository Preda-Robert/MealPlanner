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
    //var userId = User.GetUserId();

    var userId = 1; // Placeholder for user ID, replace with actual logic to get the user ID

    var result = await _unitOfServices.MealPlanService.GetMealPlanByDateRange(startDate, endDate, userId);
    if (result.Value == null) return NotFound("No meal plan for the selected week.");
    return Ok(result.Value);
  }




}
