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
  public class MealPlansControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly MealPlansController _controller;

    public MealPlansControllerTests()
    {
      _controller = new MealPlansController(_mockUnitOfServices.Object);
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
    public async Task CreateMealPlan_ReturnsOk_WhenSuccessful()
    {
      // Arrange
      var dto = new MealPlanDTO { Id = 1, Name = "Weekly Plan", Description = "Test meal plan" };
      _mockUnitOfServices.Setup(s => s.MealPlanService.Create(It.IsAny<MealPlanDTO>()))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(dto));

      // Act
      var result = await _controller.CreateMealPlan(dto);

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task CreateMealPlan_ReturnsBadRequest_WhenServiceReturnsBadRequest()
    {
      // Arrange
      var dto = new MealPlanDTO { Name = "Invalid Plan", Description = "fljse" };
      _mockUnitOfServices.Setup(s => s.MealPlanService.Create(It.IsAny<MealPlanDTO>()))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(new BadRequestObjectResult("Validation failed")));

      // Act
      var result = await _controller.CreateMealPlan(dto);

      // Assert
      result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetMealPlan_ReturnsOk_WhenPlanExists()
    {
      // Arrange
      var dto = new MealPlanDTO { Id = 1, UserId = 1, Name = "Test Plan", Description = "fafasasz" };
      _mockUnitOfServices.Setup(s => s.MealPlanService.GetByIdAsync(1))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(dto));

      // Act
      var result = await _controller.GetMealPlan(1);

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetMealPlan_ReturnsNotFound_WhenPlanDoesNotExist()
    {
      // Arrange
      _mockUnitOfServices.Setup(s => s.MealPlanService.GetByIdAsync(1))
          .ReturnsAsync(new ActionResult<MealPlanDTO>((MealPlanDTO)null!));

      // Act
      var result = await _controller.GetMealPlan(1);

      // Assert
      result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetMealPlan_ReturnsForbid_WhenUserDoesNotOwnPlan()
    {
      // Arrange
      var dto = new MealPlanDTO { Id = 1, UserId = 99, Name = "Other User Plan", Description = "Brr Brr Patapim" };
      _mockUnitOfServices.Setup(s => s.MealPlanService.GetByIdAsync(1))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(dto));

      // Act
      var result = await _controller.GetMealPlan(1);

      // Assert
      result.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task UpdateMealPlan_ReturnsOk_WhenSuccessful()
    {
      // Arrange
      var existingDto = new MealPlanDTO { Id = 1, UserId = 1, Name = "Original Plan", Description = "Baninin" };
      var updatedDto = new MealPlanDTO { Id = 1, UserId = 1, Name = "Updated Plan", Description = "aaa" };

      _mockUnitOfServices.Setup(s => s.MealPlanService.GetByIdAsync(1))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(existingDto));
      _mockUnitOfServices.Setup(s => s.MealPlanService.UpdateAsync(1, It.IsAny<MealPlanDTO>()))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(updatedDto));

      // Act
      var result = await _controller.UpdateMealPlan(1, updatedDto);

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(updatedDto);
    }

    [Fact]
    public async Task DeleteMealPlan_ReturnsNoContent_WhenSuccessful()
    {
      // Arrange
      var dto = new MealPlanDTO { Id = 1, UserId = 1, Name = "Plan to Delete", Description = "Bombardino Crocodilo" };
      _mockUnitOfServices.Setup(s => s.MealPlanService.GetByIdAsync(1))
          .ReturnsAsync(new ActionResult<MealPlanDTO>(dto));
      _mockUnitOfServices.Setup(s => s.MealPlanService.DeleteAsync(1))
          .ReturnsAsync(new ActionResult<bool>(true));

      // Act
      var result = await _controller.DeleteMealPlan(1);

      // Assert
      result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetMealPlanByDateRange_ReturnsOk_WithMealPlan()
    {
      // Arrange
      var startDate = DateTime.Today;
      var endDate = DateTime.Today.AddDays(7);
      var dto = new MealPlanDTO { Id = 1, UserId = 1, Name = "Weekly Plan", Description = "Tralalero Tralala" };

      _mockUnitOfServices.Setup(s => s.MealPlanService.GetMealPlanByDateRange(startDate, endDate, 1))
          .ReturnsAsync(new ActionResult<MealPlanDTO?>(dto));

      // Act
      var result = await _controller.GetMealPlanByDateRange(startDate, endDate);

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetMealPlans_ReturnsOk_WithPagedList()
    {
      // Arrange
      var mealPlans = new List<MealPlanDTO>
      {
        new MealPlanDTO { Id = 1, UserId = 1, Name = "Plan 1", Description = "Brr Brr Patapim" },
        new MealPlanDTO { Id = 2, UserId = 1, Name = "Plan 2" , Description = "Bombardino Crocodilo"}
      };
      var pagedList = new PagedList<MealPlanDTO>(mealPlans, mealPlans.Count, 1, 1);

      _mockUnitOfServices.Setup(s => s.MealPlanService.GetAllAsync(It.IsAny<MealPlanParams>()))
          .ReturnsAsync(new ActionResult<PagedList<MealPlanDTO>>(pagedList));

      // Act
      var result = await _controller.GetMealPlans(new MealPlanParams());

      // Assert
      var okResult = result.Result as OkObjectResult;
      okResult.Should().NotBeNull();
      okResult!.Value.Should().BeEquivalentTo(pagedList);
    }
  }
}