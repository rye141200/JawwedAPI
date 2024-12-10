using JawwedAPI.WebAPI.Extensions;

namespace JawwedAPI.WebAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddConnection(builder.Configuration).AddRepository().AddAutoMapper().AddSeedService();
        
        var app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        // app.MapGet("/", (HttpContext context) => );

        app.Run();
    }
}
