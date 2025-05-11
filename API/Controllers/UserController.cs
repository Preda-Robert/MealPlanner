using System;
using API.Interfaces;

namespace API.Controllers;

public class UserController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;

    public UserController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }

    
}
