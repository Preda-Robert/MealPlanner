using API.Data;
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
    // services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
    // services.AddScoped<IPhotoService, PhotoService>();
    // services.AddScoped<IUserRepository, UserRepository>();
    // services.AddScoped<IItemRepository, ItemRepository>();
    // services.AddScoped<IPostRepository, PostRepository>();    
    // services.AddScoped<ICategoryRepository, CategoryRepository>();
    // services.AddScoped<IUnitOfWork, UnitOfWork>();
    return services;
}
}
