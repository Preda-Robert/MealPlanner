using System;
using API.Interfaces;

namespace API.Services;

public class UnitOfServices : IUnitOfServices, IDisposable
{
    // Add the services that have more functionality than the generic services here and in the interface
    // e.g. IAllergyService etc.
    /*
        e.g.
        public readonly IAllergyService AllergyService;
        public UnitOfServices(IAllergyService allergyService)
        {
            AllergyService = allergyService;
        }
        Also, for every repo or service inherit the base service or base repo as well 
        as their respective interfaces
    */
    public IAllergyService AllergyService { get; }
    public IIngredientCategoryService IngredientCategoryService { get; }
    public IIngredientService IngredientService { get; }
    private Dictionary<Type, object> _services;
    private readonly IServiceProvider _serviceProvider;
    private bool _disposed;

    public UnitOfServices(IServiceProvider serviceProvider,
        IAllergyService allergyService,
        IIngredientCategoryService ingredientCategoryService,
        IIngredientService ingredientService)
    {
        _serviceProvider = serviceProvider;
        AllergyService = allergyService;
        IngredientCategoryService = ingredientCategoryService;
        IngredientService = ingredientService;
        _services = new Dictionary<Type, object>();
    }

    public IBaseService<TEntity, TDTO> Service<TEntity, TDTO>()
         where TEntity : class
         where TDTO : class
    {
        var type = typeof(TEntity);

        if (!_services.ContainsKey(type))
        {
            var serviceType = _serviceProvider.GetService(typeof(IBaseService<TEntity, TDTO>));
            _services[type] = serviceType!;
        }

        return (IBaseService<TEntity, TDTO>)_services[type];
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _services?.Clear();
            }

            _disposed = true;
        }
    }
}
