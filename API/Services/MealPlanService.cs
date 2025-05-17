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
      // Log the error or handle it as needed
      Console.WriteLine($"No meal plan found for user {userId} between {startDate} and {endDate}");
      return null;

    }

    return _mapper.Map<MealPlanDTO>(mealPlan);
  }




}
