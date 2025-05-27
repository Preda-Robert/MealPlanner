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
        var entities = await _unitOfWork.ServingTypeRepository.GetAllAsync();
        if (entities == null || !entities.Any())
            return new NotFoundObjectResult("No serving types found");

        var dtos = _mapper.Map<List<ServingTypeDTO>>(entities);
        return dtos;
    }
}
