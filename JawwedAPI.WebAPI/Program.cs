using Hangfire;
using JawwedAPI.Core.Jobs;
using System.Diagnostics;
using JawwedAPI.WebAPI.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
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
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure middleware pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(configure: options => { });
        }

        // Enable Swagger middleware
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Jawwed API Documentation")
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithOpenApiRoutePattern("/openapi/v1.json")
                .WithLayout(ScalarLayout.Classic)
                .WithFavicon("/favicon-256.png");
        });

        app.UseStaticFiles();
        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        // using (var scope = app.Services.CreateScope())
        // {
        //     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //     await TafsirSeeding.SeedDataAsync(dbContext);
        // }

        app.Map("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
        app.Map("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
        // app.MapGet("/", async () => await app.SeedData());
        app.UseHangfireDashboard();
        RecurringJob.AddOrUpdate<PushNotificationJob>(
            "DailyReadingNotifications",
            svc => svc.CheckAndNotifyAllUsersAsync(CancellationToken.None),
            Cron.Daily
        );
        app.Run();
    }
}
