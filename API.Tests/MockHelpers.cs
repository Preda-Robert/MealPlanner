using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace API.Tests
{
  public static class MockHelpers
  {
    public static DefaultHttpContext FakeContextWithUser(int userId)
    {
      var context = new DefaultHttpContext();
      var claims = new[] { new Claim("nameid", userId.ToString()) };
      var identity = new ClaimsIdentity(claims, "Test");
      context.User = new ClaimsPrincipal(identity);
      return context;
    }
  }

}
