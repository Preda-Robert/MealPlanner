using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;

namespace API.Services;

public class MealPlanService : BaseService<MealPlan, MealPlanDTO>, IMealPlanService
{

    public MealPlanService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }
}
