using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Helpers;

public class Seed
{
    public static Task SeedRoles(RoleManager<ApplicationRole> roleManager)
    {
        var roles = new List<ApplicationRole>
        {
            new ApplicationRole { Name = "Admin" },
            new ApplicationRole { Name = "Moderator" },
            new ApplicationRole { Name = "Member" }
        };

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role.Name!).Result)
            {
                roleManager.CreateAsync(role).Wait();
            }
        }

        return Task.CompletedTask;
    }
}
