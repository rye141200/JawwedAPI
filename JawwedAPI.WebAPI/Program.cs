using System.Diagnostics;
using JawwedAPI.WebAPI.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace JawwedAPI.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder
            .Services.AddSeedService()
            .AddConnection(builder.Configuration)
            .AddRepository()
            .AddAutoMapper()
            .AddAuthenticationAndAuthorization(builder.Configuration)
            .AddCorsPolicy()
            .AddCacheServices(builder.Configuration);

        builder.Services.AddServices(builder.Configuration);

        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure middleware pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(options => { });
        }
        // Enable Swagger middleware
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Jawwed API Documentation")
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern("/openapi/v1.json")
                .WithFavicon("/favicon-256.png");
        });

        app.UseStaticFiles();
        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Map("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        // app.MapGet("/", async () => await app.SeedData());

        // app.MapGet("/quiz/seed", async () => await app.SeedData());
        app.Run();
    }
}
