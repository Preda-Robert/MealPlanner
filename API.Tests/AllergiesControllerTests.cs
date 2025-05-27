using API.Controllers;
using API.DTO;
using API.Helpers;
using API.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace API.Tests
{
  public class AllergiesControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly AllergiesController _controller;

    public AllergiesControllerTests()
    {
      _controller = new AllergiesController(_mockUnitOfServices.Object);
    }

    [Fact]
    public async Task CreateAllergy_ReturnsCreatedAllergy()
    {
      var allergyDto = new AllergyDTO { Name = "Peanuts" };
      _mockUnitOfServices.Setup(s => s.AllergyService.Create(allergyDto))
          .ReturnsAsync(new ActionResult<AllergyDTO>(allergyDto));

      var result = await _controller.CreateAllergy(allergyDto);

      result.Value.Should().BeEquivalentTo(allergyDto);
    }

    [Fact]
    public async Task GetAllergies_ReturnsList()
    {
      var mockList = new List<AllergyDTO> { new() { Name = "Peanuts" } };
      var pagedList = new PagedList<AllergyDTO>(mockList, mockList.Count, 1, 1);

      _mockUnitOfServices.Setup(s => s.AllergyService.GetAllAsync(It.IsAny<AllergyParams>()))
          .ReturnsAsync(new ActionResult<PagedList<AllergyDTO>>(pagedList));

      var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Name, "testuser")
    };
      var identity = new ClaimsIdentity(claims, "TestAuthType");
      var user = new ClaimsPrincipal(identity);
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext { User = user }
      };

      var result = await _controller.GetAllergies(new AllergyParams());

      (result.Result as OkObjectResult)!.Value.Should().BeEquivalentTo(pagedList);
    }


    [Fact]
    public async Task CreateAllergy_ReturnsBadRequest_WhenNull()
    {
      _mockUnitOfServices.Setup(s => s.AllergyService.Create(null))
          .ReturnsAsync(new BadRequestResult());

      var result = await _controller.CreateAllergy(null);

      result.Result.Should().BeOfType<BadRequestResult>();
    }
  }

}
