using System;
using System.Text.RegularExpressions;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

namespace JawwedAPI.Core.Services;

/// <summary>
/// Service for retrieving tafsir (Quranic commentary) data
/// Provides methods to fetch tafsir by chapter or individual verse
/// </summary>
public class TafsirService(
    IGenericRepository<Tafsir> TafsirRepository,
    IGenericRepository<Mofasir> MofasirRepository
) : ITafsirService
{
    /// <summary>
    /// Gets all tafsir entries for a specific chapter by a specified mofasir by a specified language
    /// </summary>
    public async Task<List<TafsirResponse>> GetChapterTafsir(
        string? mofasirID,
        string? Chapter,
        string? language
    )
    {
        //! 1) check if mofasirName, Chapter not null
        if (mofasirID == null || Chapter == null)
            throw new GlobalErrorThrower(
                400,
                "Data is missing",
                $"The provided data is missing {(mofasirID == null ? "mofasir ID" : "chapter number")}"
            );

        //! 2) check the existence of the language and the mofasir in the database
        string languageParam = language ?? "العربية";
        await ValidateMofasirAsync(mofasirID, languageParam);

        //! 3) check the number of chapter is valid
        if (
            !int.TryParse(Chapter, out int chapterNumber)
            || chapterNumber < 1
            || chapterNumber > 114
        )
            throw new GlobalErrorThrower(
                400,
                "Invalid Chapter",
                "The provided chapter is not correct. Please enter a number between 1 and 114."
            );

        //! 4) search for the needed tafasirs in database
        List<TafsirResponse> result = await TafsirRepository.GetFilteredAndProjectAsync(
            tafsir =>
                tafsir.ChapterVerseID.StartsWith($"{Chapter}:")
                && tafsir.MofasirID == Convert.ToInt32(mofasirID)
                && tafsir.Mofasir != null
                && (
                    (languageParam == "العربية" && tafsir.Mofasir.SupportsArabic)
                    || (languageParam == "English" && tafsir.Mofasir.SupportsEnglish)
                ),
            tafsir => tafsir.Mofasir!,
            tafsir => new TafsirResponse
            {
                ChapterVerseID = tafsir.ChapterVerseID,
                Text = tafsir.Text,
            }
        );

        return result;
    }

    /// <summary>
    /// Gets a specific verse tafsir by a specified mofasir
    /// </summary>
    public async Task<TafsirResponse> GetVerseTafsir(
        string? mofasirID,
        string? ChapterVerseID,
        string? language
    )
    {
        //! 1) check if mofasirName, Chapter not null
        if (mofasirID == null || ChapterVerseID == null)
            throw new GlobalErrorThrower(
                400,
                "Data is missing",
                $"The provided data is missing {(mofasirID == null ? "mofasir ID" : "chapter or verse number")}"
            );

        //! 2) check the existence of the language and the mofasir in the database
        string languageParam = language ?? "العربية";
        await ValidateMofasirAsync(mofasirID, languageParam);

        //! 3) check the number of chapter and verse are valid
        //! 3.1) Fast validation of format before expensive parsing
        if (!ChapterVerseID.Contains(":"))
            throw new GlobalErrorThrower(
                400,
                "Invalid Format",
                "Chapter verse ID must be in format 'chapter:verse'"
            );

        //! 3.2) parsing
        string[] ChapterAndVerse = ChapterVerseID.Split(':');
        if (
            ChapterAndVerse.Length != 2
            || !int.TryParse(ChapterAndVerse[0], out int chapter)
            || !int.TryParse(ChapterAndVerse[1], out int verse)
            || chapter < 1
            || chapter > 114
            || verse < 1
            || verse > 286
        )
        {
            throw new GlobalErrorThrower(
                400,
                "Invalid Chapter or Verse",
                "The provided chapter/verse is not correct. Chapter must be between 1-114 and verse between 1-286."
            );
        }

        //! 4) search for the needed tafasir in database
        Tafsir? tafsir = await TafsirRepository.FindOneAndPopulateAsync(
            t =>
                t.ChapterVerseID == ChapterVerseID
                && t.MofasirID == Convert.ToInt32(mofasirID)
                && t.Mofasir != null,
            t => t.Mofasir!
        );

        // Create response, even if tafsir is null
        TafsirResponse result = new TafsirResponse()
        {
            ChapterVerseID = ChapterVerseID,
            Text = tafsir?.Text ?? "لا يوجد تفسير متاح لهذه الاية حاليا...",
        };

        return result;
    }

    /// <summary>
    /// Validates that the specified mofasir exists and supports the provided language
    /// </summary>
    private async Task ValidateMofasirAsync(string mofasirID, string language)
    {
        // Try to get mofasir by ID
        Mofasir? mofasir;
        if (int.TryParse(mofasirID, out int id))
        {
            mofasir = await MofasirRepository.FindOne(m => m.MofasirID == id);
        }
        else
        {
            // If not a valid integer, try to find by author name (Arabic or English)
            mofasir = await MofasirRepository.FindOne(m =>
                m.AuthorNameArabic == mofasirID || m.AuthorNameEnglish == mofasirID
            );
        }

        // Check if mofasir exists
        if (mofasir == null)
            throw new GlobalErrorThrower(
                404,
                "Mofasir Not Found",
                "The requested mofasir does not exist."
            );

        // Check language support
        bool isLanguageSupported = language switch
        {
            "العربية" => mofasir.SupportsArabic,
            "English" => mofasir.SupportsEnglish,
            _ => false,
        };

        if (!isLanguageSupported)
        {
            var authorName =
                language == "العربية" ? mofasir.AuthorNameArabic : mofasir.AuthorNameEnglish;
            throw new GlobalErrorThrower(
                404,
                "Language Not Supported",
                $"The mofasir '{authorName}' does not support the requested language."
            );
        }
    }
}
