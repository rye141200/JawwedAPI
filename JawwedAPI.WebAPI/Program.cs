using System.Reflection;
using JawwedAPI.Core.Helpers;
using JawwedAPI.Infrastructure.DbContexts;
using JawwedAPI.WebAPI.Extensions;
using Microsoft.OpenApi.Models;

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
            .AddCorsPolicy();
        builder.Services.AddServices(builder.Configuration);

        // Add Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Jawwed API",
                    Version = "v1",
                    Description = "API for Quran Jawwed Application",
                }
            );

            // Enable XML comments
            /* var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath); */
        });

        var app = builder.Build();

        // Configure middleware pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(_ => { });
        }

        // Enable Swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jawwed API V1");
        });
        app.UseStaticFiles();
        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        // app.MapGet("/", async () => await app.SeedData());

        app.Run();
    }
}
