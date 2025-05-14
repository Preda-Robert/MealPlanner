using System;
using API.Interfaces;

namespace API.Controllers;

public class MealPlanController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;
    public MealPlanController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }

    
}
