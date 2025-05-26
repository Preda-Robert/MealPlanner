using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class ServingTypeService : BaseService<ServingType, ServingTypeDTO>, IServingTypeService
{
    public ServingTypeService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }

    public override async Task<ActionResult<ICollection<ServingTypeDTO>>> GetAllAsync()
    {
        var servingTypes = await _unitOfWork.ServingTypeRepository.GetAllAsync();
        var servingTypeDTOs = _mapper.Map<ICollection<ServingTypeDTO>>(servingTypes);
        return new OkObjectResult(servingTypeDTOs);
    }
}
