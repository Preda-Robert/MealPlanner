using System;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class AllergyService(IUnitOfWork _unitOfWork, IMapper _mapper) : BaseService<Allergy, AllergyDTO>(_unitOfWork, _mapper), IAllergyService
{
}
