using API.Controllers;
using API.DTO;
using API.Helpers;
using API.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
  public class CookwaresControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly CookwaresController _controller;

    public CookwaresControllerTests()
    {
      _controller = new CookwaresController(_mockUnitOfServices.Object);
    }

    [Fact]
    public async Task CreateCookware_ReturnsCreatedItem()
    {
      var dto = new CookwareDTO { Name = "Pan", Description = "Assasino Capuccion" };
      _mockUnitOfServices.Setup(s => s.CookwareService.Create(dto))
          .ReturnsAsync(new ActionResult<CookwareDTO>(dto));

      var result = await _controller.CreateCookware(dto);

      result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetCookwares_ReturnsCookwareList()
    {
      var list = new List<CookwareDTO>
      {
        new CookwareDTO { Name = "Pan", Description="Assasino Capuccion" },
        new CookwareDTO { Name = "Pot", Description="Ballerina Capuccina" }
      };

      var pagedList = new PagedList<CookwareDTO>(list, list.Count, 1, 1);

      _mockUnitOfServices.Setup(s => s.CookwareService.GetAllAsync(It.IsAny<CookwareParams>()))
          .ReturnsAsync(new ActionResult<PagedList<CookwareDTO>>(pagedList));

      var claims = new List<System.Security.Claims.Claim>
      {
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, "1"),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "testuser")
      };
      var identity = new System.Security.Claims.ClaimsIdentity(claims, "TestAuthType");
      var user = new System.Security.Claims.ClaimsPrincipal(identity);
      _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
      {
        HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user }
      };

      var result = await _controller.GetCookwares(new CookwareParams());

      (result.Result as OkObjectResult)!.Value.Should().BeEquivalentTo(pagedList);
    }

    [Fact]
    public async Task CreateCookware_Null_ReturnsBadRequest()
    {
      _mockUnitOfServices.Setup(s => s.CookwareService.Create(null))
          .ReturnsAsync(new BadRequestResult());

      var result = await _controller.CreateCookware(null);

      result.Result.Should().BeOfType<BadRequestResult>();
    }
  }

}
