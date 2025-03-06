using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

namespace JawwedAPI.Core.Services;

public class MushafServices(
    IGenericRepositoryMapped<Line, LineResponse> lineRepositoryMapped,
    IGenericRepositoryMapped<Verse, VerseResponse> verseRepositoryMapped,
    IGenericRepositoryMapped<Chapter, ChapterResponse> chapterRepositoryMapped
) : IMushafServices
{
    public async Task<IEnumerable<LineResponse>> GetMushafPage(int pageNumber)
    {
        //! 1) check if the data is correct
        if (pageNumber <= 0 || pageNumber > 604)
        {
            throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Invalid page number",
                detail: $"The entered page number :{pageNumber} is invalid"
            );
        }
        //! 2) Get the page
        return await lineRepositoryMapped.Find(line => line.PageNumber == pageNumber);
    }

    public async Task<VerseResponse> GetVerseByChapterNumberAndVerseNumber(
        int chapterNumber,
        int verseNumber
    )
    {
        //! 1) check if the data is correct
        if (chapterNumber <= 0 || chapterNumber > 114 || verseNumber <= 0 || verseNumber > 286)
        {
            throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Invalid chapter or verse number",
                detail: $"The entered chapter number :{chapterNumber} or verse number :{verseNumber} is invalid"
            );
        }
        //! 2) Get the verse
        VerseResponse verse =
            await verseRepositoryMapped.FindOneMapped(
                (verse) => verse.ChapterID == chapterNumber && verse.VerseNumber == verseNumber
            )
            ?? throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Verse not found",
                detail: $"The verse with chapter number ${chapterNumber} and verse number ${verseNumber} was not found."
            );
        return verse;
    }

    // public async Task<IEnumerable<VerseResponse>> GetVersesByChapterNumber(int chapterNumber){
    //     //! 1) check if the data is correct
    //     if(chapterNumber <= 0 || chapterNumber > 114)
    //     {
    //         throw new GlobalErrorThrower(
    //             statusCode: 400,
    //             title: "Invalid chapter number",
    //             detail: $"The entered chapter number :{chapterNumber} is invalid"
    //         );
    //     }
    //     //! 2) Get the verses
    //     return await verseRepositoryMapped.Find(verse => verse.ChapterID == chapterNumber);
    // }
    public async Task<ChapterResponse> GetVersesByChapterNumber(int chapterNumber)
    {
        //! 1) check if the data is correct
        if (chapterNumber <= 0 || chapterNumber > 114)
        {
            throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Invalid chapter number",
                detail: $"The entered chapter number :{chapterNumber} is invalid"
            );
        }
        //! 2) Get the verses
        return await chapterRepositoryMapped.FindOneMappedPopulated(
                chapter => chapter.ChapterID == chapterNumber,
                chapter => chapter.Verses!
            )
            ?? throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Chapter not found",
                detail: $"The chapter with chapter number ${chapterNumber} was not found."
            );
    }

    public async Task<ChapterResponse> GetChapterMetaData(int chapterNumber)
    {
        //! 1) check if the data is correct
        if (chapterNumber <= 0 || chapterNumber > 114)
        {
            throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Invalid chapter number",
                detail: $"The entered chapter number :{chapterNumber} is invalid"
            );
        }
        //! 2) Get the chapter
        ChapterResponse chapter =
            await chapterRepositoryMapped.FindOneMapped(chapter =>
                chapter.ChapterID == chapterNumber
            )
            ?? throw new GlobalErrorThrower(
                statusCode: 400,
                title: "Chapter not found",
                detail: $"The chapter with chapter number ${chapterNumber} was not found."
            );
        chapter.Verses = null;
        return chapter;
    }
}
