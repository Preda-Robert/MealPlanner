using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;

namespace API.Services;

public class CookwareService : BaseService<Cookware, CookwareDTO>, ICookwareService
{
    public CookwareService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    
}
