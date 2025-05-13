using API.DTO;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Allergy, AllergyDTO>();
        CreateMap<AllergyDTO, Allergy>();

        CreateMap<Photo, PhotoDTO>();
        CreateMap<PhotoDTO, Photo>();

        CreateMap<ApplicationUser, UserDTO>()
            .ForMember(d => d.PhotoUrl, o => o.MapFrom(p => p.Photo!.Url));

        CreateMap<IngredientCategory, IngredientCategoryDTO>();
        CreateMap<IngredientCategoryDTO, IngredientCategory>();

        CreateMap<ServingType, ServingTypeDTO>();
        CreateMap<ServingTypeDTO, ServingType>();

        CreateMap<Ingredient, IngredientDTO>();
        CreateMap<IngredientDTO, Ingredient>();

        CreateMap<Recipe, RecipeDTO>();
        CreateMap<RecipeDTO, Recipe>();

        CreateMap<RecipeIngredient, RecipeIngredientDTO>();
        CreateMap<RecipeIngredientDTO, RecipeIngredient>();

        CreateMap<RecipeInstruction, RecipeInstructionDTO>();
        CreateMap<RecipeInstructionDTO, RecipeInstruction>();

        CreateMap<RegisterDTO, ApplicationUser>();

        CreateMap<DietType, DietTypeDTO>();
        CreateMap<DietTypeDTO, DietType>();
    }
}
