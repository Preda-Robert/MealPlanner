using API.DTO;
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

        CreateMap<IngredientCategory, IngredientCategoryDTO>();
        CreateMap<IngredientCategoryDTO, IngredientCategory>();

        CreateMap<Ingredient, IngredientDTO>();
        CreateMap<IngredientDTO, Ingredient>();
    }
}
