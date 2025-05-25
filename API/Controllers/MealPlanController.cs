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
    try
    {
      // Set the current user ID
      mealPlan.UserId = User.GetUserId();

      var mealPlanServiceResult = await _unitOfServices.MealPlanService.Create(mealPlan);

      if (mealPlanServiceResult.Result is BadRequestObjectResult badRequest)
      {
        return BadRequest(badRequest.Value);
      }

      return Ok(mealPlanServiceResult.Value);
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error creating meal plan: {ex.Message}");
    }
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<MealPlanDTO>> UpdateMealPlan(int id, [FromBody] MealPlanDTO mealPlan)
  {
    try
    {
      // Ensure the meal plan belongs to the current user
      var userId = User.GetUserId();
      var existingMealPlan = await _unitOfServices.MealPlanService.GetByIdAsync(id);

      if (existingMealPlan.Value == null)
      {
        return NotFound("Meal plan not found");
      }

      if (existingMealPlan.Value.UserId != userId)
      {
        return Forbid("You can only update your own meal plans");
      }

      mealPlan.Id = id;
      mealPlan.UserId = userId;

      var mealPlanServiceResult = await _unitOfServices.MealPlanService.UpdateAsync(id, mealPlan);

      if (mealPlanServiceResult.Result is BadRequestObjectResult badRequest)
      {
        return BadRequest(badRequest.Value);
      }

      return Ok(mealPlanServiceResult.Value);
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error updating meal plan: {ex.Message}");
    }
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteMealPlan(int id)
  {
    try
    {
      // Ensure the meal plan belongs to the current user
      var userId = User.GetUserId();
      var existingMealPlan = await _unitOfServices.MealPlanService.GetByIdAsync(id);

      if (existingMealPlan.Value == null)
      {
        return NotFound("Meal plan not found");
      }

      if (existingMealPlan.Value.UserId != userId)
      {
        return Forbid("You can only delete your own meal plans");
      }

      var result = await _unitOfServices.MealPlanService.DeleteAsync(id);

      if (result.Result is BadRequestObjectResult badRequest)
      {
        return BadRequest(badRequest.Value);
      }

      return NoContent();
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error deleting meal plan: {ex.Message}");
    }
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

  [HttpGet("{id}")]
  public async Task<ActionResult<MealPlanDTO>> GetMealPlan(int id)
  {
    try
    {
      var userId = User.GetUserId();
      var result = await _unitOfServices.MealPlanService.GetByIdAsync(id);

      if (result.Value == null)
      {
        return NotFound("Meal plan not found");
      }

      if (result.Value.UserId != userId)
      {
        return Forbid("You can only access your own meal plans");
      }

      return Ok(result.Value);
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error retrieving meal plan: {ex.Message}");
    }
  }
}