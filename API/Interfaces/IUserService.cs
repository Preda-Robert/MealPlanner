using System;
using API.DTO;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public interface IUserService : IBaseService<ApplicationUser, UserDTO>
{

}
