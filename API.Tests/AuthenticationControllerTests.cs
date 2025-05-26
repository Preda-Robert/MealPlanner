using API.Controllers;
using API.DTO;
using API.DTOs;
using API.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
  public class AuthenticationControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
      _mockUnitOfServices.Setup(s => s.AuthService).Returns(_mockAuthService.Object);
      _controller = new AuthenticationController(_mockUnitOfServices.Object);
    }


    [Fact]
    public async Task Register_ReturnsUserResponse()
    {
      var dto = new RegisterDTO { Email = "test@example.com", DisplayName = "Test", Password = "pass123", Username = "Tung Tung Tung Sahur" };
      var response = new RegisterResponseDTO { UserId = 1 };

      _mockUnitOfServices.Setup(s => s.AuthService.RegisterUser(dto))
          .ReturnsAsync(new ActionResult<RegisterResponseDTO>(response));

      var result = await _controller.Register(dto);

      result.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsTrue()
    {
      var dto = new EmailConfirmationDTO { Token = "abc" };

      _mockAuthService.Setup(s => s.ConfirmEmailAsync(dto))
    .ReturnsAsync(new OkObjectResult(true) as ActionResult);


      var result = await _controller.ConfirmEmail(dto);

      result.Result.Should().BeOfType<OkObjectResult>();
    }



    [Fact]
    public async Task Login_ReturnsUserDTO()
    {
      var dto = new LoginDTO { UsernameOrEmail = "test@example.com", Password = "pass" };
      var user = new UserDTO { DisplayName = "Test", Token = "pass123", UserName = "Tung Tung Tung Sahur" };

      _mockUnitOfServices.Setup(s => s.AuthService.LoginUser(dto))
          .ReturnsAsync(user);

      var result = await _controller.Login(dto);

      result.Value.Should().BeEquivalentTo(user);
    }
  }

}
