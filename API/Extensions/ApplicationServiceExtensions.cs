using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
{
    services.AddControllers().AddJsonOptions(options => {
        //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
    services.AddDbContext<DataContext>(opt => 
    {
        opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });
    services.AddCors();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    // Services
    services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
    services.AddScoped<IAllergyService, AllergyService>();
    services.AddScoped<IIngredientService, IngredientService>();
    services.AddScoped<IIngredientCategoryService, IngredientCategoryService>();
    // Repositories
    services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IUnitOfServices, UnitOfServices>();
    services.AddScoped<IAllergyRepository, AllergyRepository>();
    services.AddScoped<IIngredientRepository, IngredientRepository>();
    services.AddScoped<IIngredientCategoryRepository, IngredientCategoryRepository>();
    return services;
}
}
