using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAllergyRepository AllergyRepository { get; }
        IIngredientCategoryRepository IngredientCategoryRepository { get; }
        IIngredientRepository IngredientRepository { get; }
        IRecipeRepository RecipeRepository { get; }
        IServingTypeRepository ServingTypeRepository { get; }
        ICookwareRepository CookwareRepository { get; }

        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<bool> SaveAsync();
    }
}