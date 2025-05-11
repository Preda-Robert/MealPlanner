using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Helpers;

public class Seed
{
    public static Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "Admin" },
            new IdentityRole { Name = "Moderator" },
            new IdentityRole { Name = "Member" }
        };

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role.Name).Result)
            {
                roleManager.CreateAsync(role).Wait();
            }
        }

        return Task.CompletedTask;
    }
}
