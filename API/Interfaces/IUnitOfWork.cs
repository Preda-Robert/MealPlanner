using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAllergyRepository AllergyRepository { get; }
        IIngredientCategoryRepository IngredientCategoryRepository { get; }
        IIngredientRepository IngredientRepository { get; }

        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<bool> SaveAsync();
    }
}