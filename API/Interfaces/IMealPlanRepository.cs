using System;
using System.Linq.Expressions;  
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMealPlanRepository : IBaseRepository<MealPlan>
{
    IQueryable<MealPlan> GetMealPlans(MealPlanParams mealPlanParams);

    Task<MealPlan?> GetMealPlanByDateRange(DateTime startDate, DateTime endDate, int userId);



}
