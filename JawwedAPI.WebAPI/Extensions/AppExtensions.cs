using System;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.ServiceInterfaces.SeedInterfaces;
using JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

namespace JawwedAPI.WebAPI.Extensions;

public static class AppExtensions
{
    public static async Task SeedData(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            // var lineSeedService = scope.ServiceProvider.GetRequiredService<
            //     IGenericSeedService<JsonLine, Line>
            // >();
            // await lineSeedService.SaveToDatabase(
            //     @"E:\Senior 1 CSE\GP\GP-Backend\JawwedAPI\JawwedAPI.Infrastructure\DataSeeding\Assets\linesPopualtedUthmani.json"
            // );
            //var chapterSeedService = scope.ServiceProvider.GetRequiredService<IGenericSeedService<JsonChapter, Chapter>>();
            //await chapterSeedService.SaveToDatabase(@"E:\Senior 1 CSE\GP\GP-Backend\JawwedAPI\JawwedAPI.Infrastructure\DataSeeding\Assets\chaptersInfo.json"); */
            //var verseSeedService = scope.ServiceProvider.GetRequiredService<IGenericSeedService<JsonVerse, Verse>>();
            //await verseSeedService.SaveToDatabase(@"E:\Senior 1 CSE\GP\GP-Backend\JawwedAPI\JawwedAPI.Infrastructure\DataSeeding\Assets\versesFlattenedPerPage.json");

            var azkarSeedService = scope.ServiceProvider.GetRequiredService<
                IGenericSeedService<JsonZekr, Zekr>
            >();
            await azkarSeedService.SaveToDatabase(
                @"D:\Study\ASU\GP\JawwedAPI\JawwedAPI.Infrastructure\DataSeeding\Assets\azkar.json"
            );
        }
    }
}
