using JawwedAPI.Core.Helpers;
using JawwedAPI.Infrastructure.DbContexts;
using JawwedAPI.WebAPI.Extensions;

namespace JawwedAPI.WebAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSeedService().AddConnection(builder.Configuration).AddRepository().AddAutoMapper();
        builder.Services.AddServices(builder.Configuration);


        var app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        // app.SeedData();
        // app.MapGet("/", (HttpContext context) => );

        app.Run();
    }
}
