using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IBaseService<TEntity, TDTO> 
        where TEntity : class 
        where TDTO : class
    {
        Task<TDTO> Create(TDTO dto);
        Task<TDTO> Update(int id, TDTO dto);
        Task<bool> Delete(int id);
        Task<TDTO> GetByIdAsync(int id);
        Task<ICollection<TDTO>> GetAllAsync();
        Task<ICollection<TDTO>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
}