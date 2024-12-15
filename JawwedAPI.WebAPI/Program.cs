using JawwedAPI.Core.Helpers;
using JawwedAPI.WebAPI.Extensions;

namespace JawwedAPI.WebAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddConnection(builder.Configuration).AddRepository().AddAutoMapper().AddSeedService();
        builder.Services.AddServices(builder.Configuration);


        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(_ => { });
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();


        app.Run();
    }
}
