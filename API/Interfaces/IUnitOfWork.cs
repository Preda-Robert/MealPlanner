using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<bool> SaveAsync();
    }
}