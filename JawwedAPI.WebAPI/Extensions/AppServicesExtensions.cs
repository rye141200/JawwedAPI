using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.Exceptions;
using JawwedAPI.Core.Jobs;
using JawwedAPI.Core.Options;
using JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuizInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using JawwedAPI.Core.ServiceInterfaces.SeedInterfaces;
using JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;
using JawwedAPI.Core.ServiceInterfaces.UserInterfaces;
using JawwedAPI.Core.Services;
using JawwedAPI.Infrastructure.DataSeeding;
using JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;
using JawwedAPI.Infrastructure.DbContexts;
using JawwedAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace JawwedAPI.WebAPI.Extensions;

public static class AppServicesExtensions
{
    //! Extension method for database connection
    //this IServiceCollection services, IConfiguration config
    public static IServiceCollection AddConnection(
        this IServiceCollection service,
        IConfiguration config
    )
    {
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("JawwedAPI.Infrastructure")
            );
        });
        return service;
    }

    public static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var key = System.Text.Encoding.ASCII.GetBytes(config["Authentication:JWT:Key"]!);
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Cookie.Name = "JawwedAuthCookie";
                }
            )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["JawwedAuthCookie"];
                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        return services;
    }

    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericRepositoryMapped<,>), typeof(GenericRepositoryMapped<,>));
        return services;
    }

    public static IServiceCollection AddCacheServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        /* services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints =
                {
                    {
                        config.GetConnectionString("Redis")!,
                        int.Parse(config["RedisCredentials:Port"]!)
                    },
                },
                Password = config["RedisCredentials:Password"],
            };
            options.InstanceName = "JawwedAPI:";
        }); */

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
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

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        //!1) Custom Services
        services.AddScoped<IBookmarkServices, BookmarkServices>();
        services.AddScoped<IGoalsService, GoalsService>();
        services.AddScoped<IMushafServices, MushafServices>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITafsirService, TafsirService>();
        services.AddScoped<IMofasirService, MofasirService>();
        services.AddScoped<IAzkarService, AzkarService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IUserService, UserService>();

        //!2) Options pattern
        services.Configure<AudioAssetsOptions>(config.GetSection("AudioAssets"));
        services.Configure<JwtOptions>(config.GetSection("Authentication").GetSection("JWT"));
        services.Configure<GoogleAuthenticationOptions>(
            config.GetSection("Authentication").GetSection("Google")
        );

        //!3) Error handler
        services.AddExceptionHandler<GlobalErrorHandler>();
        services.AddProblemDetails();
        services.AddHangfire(
            (sp, globalConfig) =>
            {
                globalConfig.UseSqlServerStorage(config.GetConnectionString("DefaultConnection"));
            }
        );
        services.AddHangfireServer();
        services.AddSingleton(sp =>
        {
            var env = sp.GetRequiredService<IHostEnvironment>();
            var credsPath = Path.Combine(env.ContentRootPath, "fireBaseCredits.json");

            if (!File.Exists(credsPath))
            {
                throw new FileNotFoundException(
                    $"Firebase credentials file not found at {credsPath}. Please ensure the file exists and contains valid service account credentials."
                );
            }

            try
            {
                // Load the service‑account credential
                var credential = GoogleCredential.FromFile(credsPath);

                // Pull project_id from the same JSON
                using var doc = JsonDocument.Parse(File.ReadAllText(credsPath));
                var projectId = doc.RootElement.GetProperty("project_id").GetString();

                if (string.IsNullOrEmpty(projectId))
                {
                    throw new InvalidOperationException(
                        "Project ID not found in Firebase credentials file."
                    );
                }

                if (projectId != "jawwedapi-2cfdf")
                {
                    throw new InvalidOperationException(
                        $"Project ID mismatch. Expected 'jawwedapi-2cfdf' but found '{projectId}'."
                    );
                }

                // Create the FirebaseApp with both Credential AND ProjectId
                var app = FirebaseApp.Create(
                    new AppOptions
                    {
                        Credential = credential,
                        ProjectId = projectId,
                        ServiceAccountId = doc.RootElement.GetProperty("client_email").GetString(),
                    }
                );

                return FirebaseMessaging.GetMessaging(app);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Failed to initialize Firebase. Please check your credentials file and ensure it's valid.",
                    ex
                );
            }
        });
        services.AddScoped<INotificationService, FcmNotificationService>();
        services.AddScoped<PushNotificationJob>();
        return services;
    }
}
