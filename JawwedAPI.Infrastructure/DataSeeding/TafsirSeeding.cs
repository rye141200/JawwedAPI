using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;
using JawwedAPI.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace JawwedAPI.Infrastructure.DataSeeding;

public class TafsirSeeding
{
    public delegate void StringProcessing(params string[] lookups);
    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        // Clean existing data
        // await context.Database.ExecuteSqlRawAsync("DELETE FROM Tafsirs");
        // await context.Database.ExecuteSqlRawAsync("DELETE FROM Mofasirs");
        // await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Mofasirs', RESEED, 0)");
        // await context.SaveChangesAsync();

        //! 1) prepare the mofasir with bilingual support
        Mofasir mofasir = new Mofasir()
        {
            // Arabic information
            AuthorNameArabic = "الطبري",
            BookNameArabic = "جامع البيان عن تأويل آي القرآن",
            BiographyArabic = "محمد بن جرير الطبري، مؤرخ ومفسر وفقيه، صاحب أحد أشهر تفاسير القرآن الكريم.",

            // English information
            AuthorNameEnglish = "Al-Tabari",
            BookNameEnglish = "Jami' al-Bayan 'an Ta'wil Ayi al-Quran",
            BiographyEnglish = "Muhammad ibn Jarir al-Tabari was a historian, exegete, and jurist, known for his monumental Quranic commentary.",

            // Language support
            SupportsArabic = true,
            SupportsEnglish = true,

            // Additional biographical information
            BirthYear = "224 هـ",
            DeathYear = "310 هـ"
        };

        // Check if this mofasir already exists (by Arabic name)
        Mofasir? existedMofasir = await context.Mofasirs
            .FirstOrDefaultAsync(m => m.AuthorNameArabic == mofasir.AuthorNameArabic);

        if (existedMofasir == null)
        {
            // Add the new mofasir to the database
            await context.Mofasirs.AddAsync(mofasir);
            await context.SaveChangesAsync();
        }
        else
        {
            // Use existing mofasir
            mofasir = existedMofasir;

            // // If this mofasir already has content, we can exit early
            if (await context.Tafsirs.AnyAsync(t => t.MofasirID == mofasir.MofasirID))
            {
                return;
            }
        }

        //! 2) prepare the data for seeding
        var jsonText = await File.ReadAllTextAsync(@"F:\Programming\Csharp\JawwedAPIv3\JawwedAPI\JawwedAPI.Infrastructure\DataSeeding\Assets\Tafsir al-Tabari Processed.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        List<JsonTafsir>? jsonTafsirs = JsonSerializer.Deserialize<List<JsonTafsir>>(jsonText, options);

        if (jsonTafsirs == null)
        {
            throw new InvalidOperationException("Failed to deserialize tafsir JSON data");
        }

        //! 3) feed the data to Tafsir table
        foreach (var tafsir in jsonTafsirs)
        {
            await context.Tafsirs.AddAsync(new Tafsir()
            {
                ChapterVerseID = tafsir.ChapterVerseID,
                MofasirID = mofasir.MofasirID,
                Text = tafsir.Text ?? "لا يوجد تفسير لهذه الاية"
            });

            // Save in batches to improve performance
            if (context.ChangeTracker.Entries().Count() > 100)
            {
                await context.SaveChangesAsync();
            }
        }

        // Save any remaining changes
        await context.SaveChangesAsync();
    }
    public static async Task DeleteSpeecificEntries<T>(Expression<Func<T, bool>> removeExpression, ApplicationDbContext context) where T : class
    {
        List<T> values = context.Set<T>().Where(removeExpression).ToList();
        context.Set<T>().RemoveRange(values);
        await context.SaveChangesAsync();
    }
}