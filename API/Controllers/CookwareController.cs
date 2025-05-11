using System;
using API.DTO;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CookwareController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;

    public CookwareController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }

    [HttpPost]
    public async Task<ActionResult<CookwareDTO>> CreateCookware([FromBody] CookwareDTO cookwareDTO)
    {
        var cookwareCreationResult = await _unitOfServices.CookwareService.Create(cookwareDTO);
        return cookwareCreationResult;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CookwareDTO>>> GetAllCookware()
    {
        var cookwareList = await _unitOfServices.CookwareService.GetAllAsync();
        return Ok(cookwareList);
    }
}
