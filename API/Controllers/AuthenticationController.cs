using System;
using API.DTO;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthenticationController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;

    public AuthenticationController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var userServiceResult = await _unitOfServices.UserService.LoginUser(loginDTO);
        return userServiceResult;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterDTO registerDTO)
    {
        var userServiceResult = await _unitOfServices.UserService.RegisterUser(registerDTO);
        return userServiceResult;
    }

    [HttpPost("google")]
    public async Task<ActionResult<UserDTO>> GoogleAuth(GoogleDTO googleDTO)
    {
        var userServiceResult = await _unitOfServices.UserService.GoogleAuth(googleDTO);
        return userServiceResult;
    }

    [HttpPost("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(EmailConfirmationDTO emailConfirmationDTO)
    {
        var userServiceResult = await _unitOfServices.UserService.ConfirmEmailAsync(emailConfirmationDTO);
        return userServiceResult;
    }
}
