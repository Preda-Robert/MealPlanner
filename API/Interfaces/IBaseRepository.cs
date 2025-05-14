using System.Linq.Expressions;

namespace API.Interfaces
{
  public interface IBaseRepository<TEntity> where TEntity : class
  {
    Task<TEntity> GetByIdAsync(int id);
    Task<ICollection<TEntity>> GetAllAsync();

    Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null!);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
  }
}