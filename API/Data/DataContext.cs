using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options) //specific order for this one
{
    public DbSet<Cookware> Cookwares { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<IngredientCategory> IngredientCategories { get; set; }
    public DbSet<DietaryPreferences> DietaryPreferences { get; set; }
    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<Photo> Photos { get; set; }

    // override conventions for entity framework
    protected override void OnModelCreating(ModelBuilder builder)
    {
         base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
        builder.Entity<ApplicationRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.Entity<Recipe>()
        .HasMany(r => r.Ingredients)
            .WithOne(ri => ri.Recipe)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Recipe>()
            .HasMany(r => r.Instructions)
            .WithOne(rc => rc.Recipe)
            .HasForeignKey(rc => rc.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Recipe>()
            .HasOne(r => r.ServingType)
            .WithMany(s => s.Recipes)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ServingType>()
            .HasMany(st => st.Recipes)
            .WithOne(r => r.ServingType)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Ingredient>()
            .HasMany(i => i.RecipeIngredients)
            .WithOne(ri => ri.Ingredient)
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Recipe>()
            .HasMany(r => r.Cookware)
            .WithOne(rc => rc.Recipe)
            .HasForeignKey(rc => rc.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
            
            
    }
}
