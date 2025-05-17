using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MealPlanRepository : BaseRepository<MealPlan>, IMealPlanRepository
{
    public MealPlanRepository(DataContext context) : base(context)
    {
    }

    public IQueryable<MealPlan> GetMealPlans(MealPlanParams mealPlanParams)
    {
        var query = _context.MealPlans.AsQueryable();

        if (!string.IsNullOrEmpty(mealPlanParams.SearchTerm))
        {
            query = query.Where(m => m.Name.ToLower().Contains(mealPlanParams.SearchTerm.ToLower()));
        }

        query = query.OrderBy(m => m.Name);

        return query;
    }

    public async Task<MealPlan?> GetMealPlanByDateRange(DateTime startDate, DateTime endDate, int userId)
{
    return await _context.MealPlans
        .Include(m => m.MealPlanRecipes)
            .ThenInclude(r => r.Recipe)
        .Include(m => m.MealPlanRecipes)
            .ThenInclude(r => r.ServingType)
        .FirstOrDefaultAsync(m => m.StartDate.Date == startDate.Date && m.EndDate.Date == endDate.Date && m.UserId == userId);
}

    
}
