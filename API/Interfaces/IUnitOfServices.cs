using System;

namespace API.Interfaces;

public interface IUnitOfServices : IDisposable
{
    IBaseService<TEntity, TDTO> Service<TEntity, TDTO>()
        where TEntity : class
        where TDTO : class;
}
