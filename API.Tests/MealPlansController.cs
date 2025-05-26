//using API.DTO;
//using API.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace API.Tests;

//public class MealPlansController
//{
//  [Fact]
//  public async Task CreateMealPlan_ReturnsOk_WhenSuccessful()
//  {
//    var mockService = new Mock<IUnitOfServices>();
//    var dto = new MealPlanDTO { Id = 1, UserId = 1, Name = "Test", Description = "Test Desc" };
//    mockService.Setup(x => x.MealPlanService.Create(It.IsAny<MealPlanDTO>()))
//               .ReturnsAsync(ActionResultWrapper.Ok(dto));

//    var controller = new MealPlansController(mockService.Object);
//    controller.ControllerContext.HttpContext = MockHelpers.FakeContextWithUser(1);

//    var result = await controller.CreateMealPlan(dto);

//    var okResult = Assert.IsType<OkObjectResult>(result.Result);
//    var returnedDto = Assert.IsType<MealPlanDTO>(okResult.Value);
//    Assert.Equal(1, returnedDto.Id);
//  }

//  [Fact]
//  public async Task GetMealPlan_ReturnsForbidden_WhenWrongUser()
//  {
//    var mockService = new Mock<IUnitOfServices>();
//    var dto = new MealPlanDTO { Id = 1, UserId = 99 };
//    mockService.Setup(x => x.MealPlanService.GetByIdAsync(1))
//               .ReturnsAsync(ActionResultWrapper.Ok(dto));

//    var controller = new MealPlansController(mockService.Object);
//    controller.ControllerContext.HttpContext = MockHelpers.FakeContextWithUser(1);

//    var result = await controller.GetMealPlan(1);
//    Assert.IsType<ForbidResult>(result.Result);
//  }

//  [Fact]
//  public async Task DeleteMealPlan_ReturnsNotFound_WhenMealPlanMissing()
//  {
//    var mockService = new Mock<IUnitOfServices>();
//    mockService.Setup(x => x.MealPlanService.GetByIdAsync(1))
//               .ReturnsAsync(ActionResultWrapper.Ok<MealPlanDTO>(null));

//    var controller = new MealPlansController(mockService.Object);
//    controller.ControllerContext.HttpContext = MockHelpers.FakeContextWithUser(1);

//    var result = await controller.DeleteMealPlan(1);
//    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//    Assert.Equal("Meal plan not found", notFoundResult.Value);
//  }

//  [Fact]
//  public async Task GetMealPlanByDateRange_ReturnsOk()
//  {
//    var mockService = new Mock<IUnitOfServices>();
//    var dto = new MealPlanDTO { Id = 1, UserId = 1 };
//    mockService.Setup(x => x.MealPlanService.GetMealPlanByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
//               .ReturnsAsync(ActionResultWrapper.Ok(dto));

//    var controller = new MealPlansController(mockService.Object);
//    controller.ControllerContext.HttpContext = MockHelpers.FakeContextWithUser(1);

//    var result = await controller.GetMealPlanByDateRange(DateTime.Today, DateTime.Today.AddDays(7));
//    var okResult = Assert.IsType<OkObjectResult>(result.Result);
//    Assert.NotNull(okResult.Value);
//  }

//}
