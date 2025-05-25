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
