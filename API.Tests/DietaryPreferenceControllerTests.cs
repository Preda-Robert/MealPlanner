using API.Controllers;
using API.DTO;
using API.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
  public class DietaryPreferenceControllerTests
  {
    private readonly Mock<IUnitOfServices> _mockUnitOfServices = new();
    private readonly DietaryPreferenceController _controller;

    public DietaryPreferenceControllerTests()
    {
      _controller = new DietaryPreferenceController(_mockUnitOfServices.Object);
    }

    [Fact]
    public async Task CreateDietaryPreference_ReturnsCreatedPreference()
    {
      var dto = new DietaryPreferenceDTO { DietType = new DietTypeDTO { Name = "Vegan" } };

      _mockUnitOfServices.Setup(s => s.DietaryPreferenceService.Create(dto))
          .ReturnsAsync(new ActionResult<DietaryPreferenceDTO>(dto));

      var result = await _controller.CreateDietaryPreference(dto);

      result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetDietaryPreferences_ReturnsList()
    {
      var list = new List<DietaryPreferenceDTO> {
      new DietaryPreferenceDTO { DietType = new DietTypeDTO { Name = "Keto" } }

      };
      _mockUnitOfServices.Setup(s => s.DietaryPreferenceService.GetAllAsync())
          .ReturnsAsync(new ActionResult<ICollection<DietaryPreferenceDTO>>(list));

      var result = await _controller.GetDietaryPreferences();

      result.Value.Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task SaveDietaryPreference_ReturnsOk()
    {
      var dto = new SaveDietPreferenceDTO { AllergyIds = new List<int> { 1, 2 } };
      _mockUnitOfServices.Setup(s => s.DietaryPreferenceService.SaveDietaryPreference(It.IsAny<int>(), dto))
          .ReturnsAsync(new OkResult());

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
      var result = await _controller.SaveDietaryPreference(dto);

      result.Should().BeOfType<OkResult>();
    }
  }

}
