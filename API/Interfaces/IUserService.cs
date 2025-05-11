using System;
using API.DTO;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IUserService : IBaseService<ApplicationUser, UserDTO>
{
    Task<bool> ValidateUserCredentialsAsync(ApplicationUser user, string password);
    Task<ActionResult<RegisterResponseDTO>> RegisterUser(RegisterDTO registerDTO);
    Task<ActionResult<UserDTO>> LoginUser(LoginDTO loginDTO);
    Task<ActionResult<UserDTO>> GoogleAuth(GoogleDTO googleDTO);
    Task<ActionResult> ConfirmEmailAsync(EmailConfirmationDTO emailConfirmationDTO);
}
