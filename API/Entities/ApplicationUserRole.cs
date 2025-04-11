using System;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class ApplicationUserRole : IdentityUserRole<int>
{
    public ApplicationUser User { get; set; } = null!;
    public ApplicationRole Role { get; set; } = null!;
}
