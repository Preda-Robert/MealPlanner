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
    services.AddControllers();
    services.AddDbContext<DataContext>(opt => 
    {
        opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });
    services.AddCors();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    // Services
    services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
    services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    // Repositories
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IUnitOfServices, UnitOfServices>();
    return services;
}
}
