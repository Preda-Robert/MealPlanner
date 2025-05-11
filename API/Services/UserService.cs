using System;
using API.DTO;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class UserService : BaseService<ApplicationUser, UserDTO>, IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;
    public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IEmailService emailService, IConfiguration config, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _config = config;
    }
    public Task<bool> ValidateUserCredentialsAsync(ApplicationUser user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }
    public async Task<ActionResult<RegisterResponseDTO>> RegisterUser(RegisterDTO registerDTO)
    {
        var existingUser = await GetUserByUsernameOrEmailAsync(registerDTO.Username!, registerDTO.Email!);

        if (existingUser != null)
        {
            if (existingUser.UserName == registerDTO.Username && existingUser.Email != registerDTO.Email)
            {
                return new BadRequestObjectResult("Username is already taken");
            }
            if (existingUser.UserName != registerDTO.Username && existingUser.Email == registerDTO.Email)
            {
                return new BadRequestObjectResult("Email is already taken");
            }
            if (existingUser.UserName == registerDTO.Username && existingUser.Email == registerDTO.Email)
            {
                return new RegisterResponseDTO
                {
                    IsRegistered = true,
                    RequiresEmailConfirmation = existingUser.EmailConfirmed == false,
                };
            }

        }

        var user = _mapper.Map<ApplicationUser>(registerDTO);
        var createResult = await _userManager.CreateAsync(user, registerDTO.Password!);
        if (createResult.Succeeded == false)
        {
            return new BadRequestObjectResult("Problem creating user account");
        }
        
        user = await _userManager.FindByNameAsync(registerDTO.Username!);
        user.UserRoles
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        SendEmailRequest sendEmailRequest = new SendEmailRequest(user.Email!,
            "Confirm your email",
            $"<h1>Confirm your email</h1><p>Click <a href='{_config["APIUrl"]}/account/confirm-email?id={user.Id}&token={emailToken}'>here</a> to confirm your email.</p>"
        );
        var emailResult = await _emailService.SendEmailAsync(sendEmailRequest);

        if (emailResult is BadRequestObjectResult badRequestResult)
        {
            return new BadRequestObjectResult("Problem sending email confirmation link: " + badRequestResult.Value);
        }

        return new RegisterResponseDTO
        {
            IsRegistered = false,
            RequiresEmailConfirmation = true
        };
    }
    public async Task<ActionResult<UserDTO>> LoginUser(LoginDTO loginDTO)
    {

        if (string.IsNullOrEmpty(loginDTO.UsernameOrEmail) || string.IsNullOrEmpty(loginDTO.Password))
        {
            return new BadRequestObjectResult("Invalid credentials");
        }

        var userByUsername = await _userManager.FindByNameAsync(loginDTO.UsernameOrEmail!);
        var userByEmail = await _userManager.FindByEmailAsync(loginDTO.UsernameOrEmail!);

        if (userByUsername != null && userByEmail != null)
        {
            return new BadRequestObjectResult("Invalid credentials");
        }

        var user = userByUsername ?? userByEmail;
        if (user == null)
        {
            return new BadRequestObjectResult("Invalid credentials");
        }

        var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (emailConfirmed == false)
        {
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            SendEmailRequest sendEmailRequest = new SendEmailRequest(user.Email!,
                "Email confirmation code",
                $"<h1>Confirm your email</h1><p>Here is your email confirmation code {emailToken}</p><p>Use this code to confirm your email.</p>"
            );
            var emailServiceResult = await _emailService.SendEmailAsync(sendEmailRequest);
            if (emailServiceResult is BadRequestObjectResult)
            {
                return new BadRequestObjectResult("Problem sending email confirmation link");
            }
            return new UnauthorizedObjectResult("Email not confirmed. Check your email for confirmation link.");
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password!);
        if (result == false)
        {
            return new UnauthorizedObjectResult("Invalid credentials");
        }

        var token = await _tokenService.CreateToken(user);

        return new UserDTO
        {
            UserName = user.UserName!,
            DisplayName = user.DisplayName,
            Token = token,
            PhotoUrl = user.Photo?.Url,
            EmailConfirmed = user.EmailConfirmed,
        };
    }
    public async Task<ActionResult<UserDTO>> GoogleAuth(GoogleDTO goolgeDTO)
    {
        var validationSettings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = [_config["Authentication:Google:ClientId"]]
        };
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(goolgeDTO.IdToken, validationSettings);
        }
        catch (Exception)
        {
            return new UnauthorizedObjectResult("Invalid credentials");
        }
        if (payload == null)
        {
            return new UnauthorizedObjectResult("Invalid credentials");
        }
        var user = await GetUserByUsernameOrEmailAsync(null!, payload.Email!);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = payload.Name,
                DisplayName = payload.Name,
                Email = payload.Email,
                EmailConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded == false)
            {
                return new BadRequestObjectResult("Problem creating user account " + result.Errors.Select(x => x.Description).ToList());
            }
        }
    
        var token = await _tokenService.CreateToken(user);

        return new UserDTO
        {
            UserName = user.UserName!,
            DisplayName = user.DisplayName,
            Token = token,
            PhotoUrl = user.Photo?.Url,
            EmailConfirmed = user.EmailConfirmed,
        };
    }
    public async Task<ActionResult> ConfirmEmailAsync(EmailConfirmationDTO emailConfirmationDTO)
    {
        var user = await _userManager.FindByIdAsync(emailConfirmationDTO.Id.ToString());
        if (user == null)
        {
            return new BadRequestObjectResult("Invalid credentials");
        }
        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationDTO.Token!);
        if (result.Succeeded == false)
        {
            return new BadRequestObjectResult("Problem confirming email");
        }
        return new OkObjectResult("Email confirmed successfully");
    }
    private async Task<ApplicationUser?> GetUserByUsernameOrEmailAsync(string? username, string? email)
    {
        ApplicationUser? user;
        if (!string.IsNullOrEmpty(username))
        {
            user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                return user;
            }
        }
        if (!string.IsNullOrEmpty(email))
        {
            user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        return null;
    }

}
