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
  public class IngredientsControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly IngredientsController _controller;

    public IngredientsControllerTests()
    {
      _controller = new IngredientsController(_mockUnitOfServices.Object);
      SetupControllerContext();
    }

    private void SetupControllerContext()
    {
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
    }

    [Fact]
    public async Task CreateIngredient_ReturnsCreatedIngredient()
    {
      // Arrange
      var dto = new IngredientDTO
      {
        Name = "Tomato",
      };
      _mockUnitOfServices.Setup(s => s.IngredientService.Create(dto))
          .ReturnsAsync(new ActionResult<IngredientDTO>(dto));

      // Act
      var result = await _controller.CreateIngredient(dto);

      // Assert
      result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task CreateIngredient_ReturnsBadRequest_WhenNull()
    {
      // Arrange
      _mockUnitOfServices.Setup(s => s.IngredientService.Create(null))
          .ReturnsAsync(new BadRequestResult());

      // Act
      var result = await _controller.CreateIngredient(null);

      // Assert
      result.Result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task GetIngredients_ReturnsPagedList()
    {
      // Arrange
      var ingredients = new List<IngredientDTO>
      {
        new IngredientDTO { Name = "Carrot",  },
        new IngredientDTO { Name = "Broccoli" }
      };
      var pagedList = new PagedList<IngredientDTO>(ingredients, ingredients.Count, 1, 1);

      _mockUnitOfServices.Setup(s => s.IngredientService.GetAllAsync(It.IsAny<IngredientParams>()))
          .ReturnsAsync(new ActionResult<PagedList<IngredientDTO>>(pagedList));

      // Act
      var result = await _controller.GetIngredients(new IngredientParams());

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(pagedList);
    }

    [Fact]
    public async Task GetIngredients_SetsCurrentUserInParams()
    {
      // Arrange
      var ingredients = new List<IngredientDTO>();
      var pagedList = new PagedList<IngredientDTO>(ingredients, 0, 1, 1);
      IngredientParams capturedParams = null;

      _mockUnitOfServices.Setup(s => s.IngredientService.GetAllAsync(It.IsAny<IngredientParams>()))
          .Callback<IngredientParams>(p => capturedParams = p)
          .ReturnsAsync(new ActionResult<PagedList<IngredientDTO>>(pagedList));

      // Act
      await _controller.GetIngredients(new IngredientParams());

      // Assert
      capturedParams.Should().NotBeNull();
      capturedParams!.CurrentUser.Should().Be(1);
    }

    [Fact]
    public async Task CreateIngredient_WithCompleteData_ReturnsSuccess()
    {
      // Arrange
      var dto = new IngredientDTO
      {
        Id = 1,
        Name = "Chicken Breast",
        Calories = 165,

      };
      _mockUnitOfServices.Setup(s => s.IngredientService.Create(dto))
          .ReturnsAsync(new ActionResult<IngredientDTO>(dto));

      // Act
      var result = await _controller.CreateIngredient(dto);

      // Assert
      result.Value.Should().BeEquivalentTo(dto);
      result.Value!.Name.Should().Be("Chicken Breast");
      result.Value.Calories.Should().Be(165);
    }
  }
}