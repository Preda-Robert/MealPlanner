using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[Route("api/diet-type")]
[ApiController]
public class DietTypeController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;
    public DietTypeController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }
    [HttpPost]
    public async Task<ActionResult<DietTypeDTO>> CreateDietType([FromBody] DietTypeDTO dietType)
    {
        var dietTypeServiceResult = await _unitOfServices.Service<DietType, DietTypeDTO>().Create(dietType);
        return dietTypeServiceResult;
    }
    [HttpGet]
    public async Task<ActionResult<ICollection<DietTypeDTO>>> GetDietTypes()
    {
        var dietTypesServiceResult = await _unitOfServices.Service<DietType, DietTypeDTO>().GetAllAsync();
        return dietTypesServiceResult;
    }
}
