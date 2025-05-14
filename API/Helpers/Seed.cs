using System;
using System.Text.Json;
using API.DTO;
using API.Entities;
using API.Interfaces;
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

    public static async Task SeedAllergies(IUnitOfServices unitOfServices)
    {
        var allergies = await File.ReadAllTextAsync("Helpers/Allergies.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };  
        var allergiesList = JsonSerializer.Deserialize<List<AllergyDTO>>(allergies, options);
        var allergiesInDbResult = await unitOfServices.AllergyService.GetAllAsync();
        var allergiesInDb = allergiesInDbResult.Value;
        if(allergiesInDb == null)
        {
            allergiesInDb = [];
        }
        else
        {
            return;
        }
        foreach (var allergy in allergiesList!)
        {
            await unitOfServices.AllergyService.Create(allergy);
        }
    }

    public static async Task SeedIngredientCategories(IUnitOfServices unitOfServices)
    {
        var categories = await File.ReadAllTextAsync("Helpers/IngredientCategories.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };  
        var categoriesList = JsonSerializer.Deserialize<List<IngredientCategoryDTO>>(categories, options);
        foreach (var category in categoriesList!)
        {
            Console.WriteLine(category.Name);
        }
        Console.Write(categoriesList);
        var categoriesInDbResult = await unitOfServices.IngredientCategoryService.GetAllAsync();
        var categoriesInDb = categoriesInDbResult.Value;
        if(categoriesInDb == null)
        {
            categoriesInDb = [];
        }
        else
        {
            return;
        }
        foreach (var category in categoriesList!)
        {
            await unitOfServices.IngredientCategoryService.Create(category);
        }
    }

    public static async Task SeedIngredients(IUnitOfServices unitOfServices)
    {
        var ingredients = await File.ReadAllTextAsync("Helpers/Ingredients.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };  
        var ingredientsList = JsonSerializer.Deserialize<List<IngredientDTO>>(ingredients, options);
        var ingredientsInDbResult = await unitOfServices.IngredientService.GetAllAsync();
        var ingredientsInDb = ingredientsInDbResult.Value;
        if(ingredientsInDb == null)
        {
            ingredientsInDb = [];
        }
        else
        {
            return;
        }
        foreach (var ingredient in ingredientsList!)
        {
            await unitOfServices.IngredientService.Create(ingredient);
        }
    }

    public static async Task SeedCookware(IUnitOfServices unitOfServices)
    {
        var cookwares = await File.ReadAllTextAsync("Helpers/Cookware.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };  
        var cookwaresList = JsonSerializer.Deserialize<List<CookwareDTO>>(cookwares, options);
        var cookwaresInDbResult = await unitOfServices.CookwareService.GetAllAsync();
        var cookwaresInDb = cookwaresInDbResult.Value;
        if(cookwaresInDb == null)
        {
            cookwaresInDb = [];
        }
        else
        {
            return;
        }
        foreach (var cookware in cookwaresList!)
        {
            await unitOfServices.CookwareService.Create(cookware);
        }
    }

    public static async Task SeedRecipes(IUnitOfServices unitOfServices)
    {
        var recipes = await File.ReadAllTextAsync("Helpers/Recipes.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };  
        var recipesList = JsonSerializer.Deserialize<List<RecipeDTO>>(recipes, options);
        var recipesInDbResult = await unitOfServices.RecipeService.GetAllAsync();
        var recipesInDb = recipesInDbResult.Value;
        if(recipesInDb == null)
        {
            recipesInDb = [];
        }
        else
        {
            return;
        }
        foreach (var recipe in recipesList!)
        {
            await unitOfServices.RecipeService.Create(recipe);
        }
    }
}
