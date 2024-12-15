using Microsoft.EntityFrameworkCore;
using JawwedAPI.Infrastructure.DbContexts;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Infrastructure.Repositories;
using JawwedAPI.Infrastructure.DataSeeding;
using JawwedAPI.Core.ServiceInterfaces.SeedInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using JawwedAPI.Core.Services;
using JawwedAPI.Core.Options;
using JawwedAPI.Core.Exceptions;

namespace JawwedAPI.WebAPI.Extensions;

public static class AppServicesExtensions
{
    //! Extension method for database connection
    //this IServiceCollection services, IConfiguration config
    public static IServiceCollection AddConnection(this IServiceCollection service, IConfiguration config)
    {
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("JawwedAPI.Infrastructure"));
        });
        return service;
    }

    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericRepositoryMapped<,>), typeof(GenericRepositoryMapped<,>));
        services.AddScoped(typeof(IGenericProcedureRepository<,>), typeof(GenericProcedureRepository<,>));

        return services;
    }
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    public static IServiceCollection AddSeedService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericSeedService<,>), typeof(GenericSeedService<,>));
        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IBookmarkServices, BookmarkServices>();
        services.Configure<AudioAssetsOptions>(config.GetSection("AudioAssets"));
        services.AddExceptionHandler<GlobalErrorHandler>();
        return services;
    }


}