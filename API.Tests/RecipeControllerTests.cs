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
  public class RecipesControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly RecipesController _controller;

    public RecipesControllerTests()
    {
      _controller = new RecipesController(_mockUnitOfServices.Object);
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
    public async Task CreateRecipe_ReturnsCreatedRecipe()
    {
      // Arrange
      var dto = new RecipeDTO
      {
        Id = 1,
        Name = "Spaghetti Carbonara",
        Description = "Classic Italian pasta dish",
        CookingTime = 20,
        Rating = 4.5f,
        Difficulty = Entities.RecipeDifficulty.Medium,
        ServingType = new ServingTypeDTO { Id = 1, Description = "Main Course" },
        Ingredients = new List<RecipeIngredientDTO>
        {
          new RecipeIngredientDTO
          {
            Quantity = 100,
            Ingredient = new IngredientDTO { Id = 1, Name = "Spaghetti" }
          }
        }
      };
      _mockUnitOfServices.Setup(s => s.RecipeService.Create(dto))
          .ReturnsAsync(new ActionResult<RecipeDTO>(dto));

      // Act
      var result = await _controller.CreateRecipe(dto);

      // Assert
      result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task CreateRecipe_ReturnsBadRequest_WhenNull()
    {
      // Arrange
      _mockUnitOfServices.Setup(s => s.RecipeService.Create(null))
          .ReturnsAsync(new BadRequestResult());

      // Act
      var result = await _controller.CreateRecipe(null);

      // Assert
      result.Result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task GetRecipes_ReturnsPagedList()
    {
      // Arrange
      var recipes = new List<RecipeDTO>
      {
        new RecipeDTO { Id = 1, Name = "Pasta Bolognese", Description = "Italian meat sauce pasta" },
        new RecipeDTO { Id = 2, Name = "Chicken Curry", Description = "Spicy Indian curry" },
        new RecipeDTO { Id = 3, Name = "Caesar Salad", Description = "Classic Roman salad" }
      };
      var pagedList = new PagedList<RecipeDTO>(recipes, recipes.Count, 1, 1);

      _mockUnitOfServices.Setup(s => s.RecipeService.GetAllAsync(It.IsAny<RecipeParams>()))
          .ReturnsAsync(new ActionResult<PagedList<RecipeDTO>>(pagedList));

      // Act
      var result = await _controller.GetRecipes(new RecipeParams());

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(pagedList);
    }

    [Fact]
    public async Task GetRecipes_WithSearchParams_ReturnsFilteredResults()
    {
      // Arrange
      var recipeParams = new RecipeParams { SearchTerm = "pasta" };
      var filteredRecipes = new List<RecipeDTO>
      {
        new RecipeDTO { Id = 1, Name = "Pasta Bolognese", Description = "Italian meat sauce pasta" }
      };
      var pagedList = new PagedList<RecipeDTO>(filteredRecipes, 1, 1, 1);

      _mockUnitOfServices.Setup(s => s.RecipeService.GetAllAsync(recipeParams))
          .ReturnsAsync(new ActionResult<PagedList<RecipeDTO>>(pagedList));

      // Act
      var result = await _controller.GetRecipes(recipeParams);

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      var returnedList = okResult!.Value as PagedList<RecipeDTO>;
      returnedList.Should().HaveCount(1);
      returnedList!.First().Name.Should().Be("Pasta Bolognese");
    }

    [Fact]
    public async Task CreateRecipe_WithCompleteData_ReturnsSuccess()
    {
      // Arrange



      var dto = new RecipeDTO
      {
        Id = 1,
        Name = "Grilled Salmon",
        Description = "Delicious grilled salmon with herbs",
        CookingTime = 30,
        Rating = 4.5f,
        Difficulty = Entities.RecipeDifficulty.Easy,
        ServingType = new ServingTypeDTO { Id = 1, Description = "Main Course", NumberOfServings = 2 },
        Ingredients = new List<RecipeIngredientDTO>
      {

      new RecipeIngredientDTO
        {
          Quantity = 200,
          Ingredient = new IngredientDTO { Id = 1, Name = "Salmon" }
        }
      }
      };
      _mockUnitOfServices.Setup(s => s.RecipeService.Create(dto))
          .ReturnsAsync(new ActionResult<RecipeDTO>(dto));

      // Act
      var result = await _controller.CreateRecipe(dto);

      // Assert
      result.Value.Should().BeEquivalentTo(dto);
      result.Value!.Name.Should().Be("Grilled Salmon");
      result.Value.ServingType.NumberOfServings.Should().Be(2);
      result.Value.Difficulty.Should().Be(Entities.RecipeDifficulty.Easy);
    }

    [Fact]
    public async Task GetRecipes_ReturnsEmptyList_WhenNoRecipes()
    {
      // Arrange
      var emptyList = new List<RecipeDTO>();
      var pagedList = new PagedList<RecipeDTO>(emptyList, 0, 1, 1);

      _mockUnitOfServices.Setup(s => s.RecipeService.GetAllAsync(It.IsAny<RecipeParams>()))
          .ReturnsAsync(new ActionResult<PagedList<RecipeDTO>>(pagedList));

      // Act
      var result = await _controller.GetRecipes(new RecipeParams());

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      var returnedList = okResult!.Value as PagedList<RecipeDTO>;
      returnedList.Should().BeEmpty();
    }
  }
}