using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;
using JawwedAPI.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace JawwedAPI.Infrastructure.DataSeeding;

public class TafsirSeeding
{
    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        //! 1) prepare the mofasir 
        Mofasir mofasir = new Mofasir()
        {
            AuthorName = "ابن كثير",
            BookName = "تفسير ابن كثير",
            Languages = "العربية"
        };
        Mofasir? existedMofasir = context.Mofasirs.FirstOrDefault(m => m.AuthorName == mofasir.AuthorName);
        if (existedMofasir == null)
        {
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Tafsirs");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Mofasirs");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Mofasirs', RESEED, 0)");
            await context.Mofasirs.AddAsync(mofasir);
            await context.SaveChangesAsync();

        }
        else if (existedMofasir.Languages != null && existedMofasir.Languages.Split(",").Contains(mofasir.Languages))
        {
            return;
        }
        //! 2) prepare the data for seeding
        var jsonText = await File.ReadAllTextAsync(@"F:\Programming\Csharp\JawwedAPIv3\JawwedAPI\JawwedAPI.Infrastructure\DataSeeding\Assets\Tafsir.json");
        List<JsonTafsir>? jsonTafsirs = JsonSerializer.Deserialize<List<JsonTafsir>>(jsonText,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //! 3) feed the data to Tafsir table
        foreach (var tafsir in jsonTafsirs)
        {
            if (tafsir.Text != null)
            {
                await context.Tafsirs.AddAsync(new Tafsir()
                {

                    ChapterVerseID = tafsir.ChapterVerseID,
                    MofasirID = tafsir.MofasirID == 1 ? 0 : 0,
                    Text = tafsir.Text
                });
            }
            else
            {
                await context.Tafsirs.AddAsync(new Tafsir()
                {

                    ChapterVerseID = tafsir.ChapterVerseID,
                    MofasirID = tafsir.MofasirID == 1 ? 0 : 0,
                    Text = "لا يوجد تفسير لهذه الاية"
                });
            }
        }
        await context.SaveChangesAsync();
    }
}