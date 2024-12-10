

using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
//IGenericSeedService<JsonLine, Line> seed
public class HomeController(IGenericProcedureRepository<Line, LineWithVerseDTO> repository) : ControllerBase
{
    [HttpGet("/api/home/{page}")]
    public async Task<IActionResult> Index([FromRoute] int page)
    {
        //await seed.SaveToDatabase(@"E:\Senior 1 CSE\GP\GP-Backend\JawwedAPI\DAL\Seed\Assets\linesPopulatedUthmani.json");
        //!1) Getting the DTO
        var list = (await repository.ExecuteStoredProcedure(page, "GetLineWithVerses", "PageNumber")).
        OrderBy(lineWithVerse => lineWithVerse.LineNumber)
        .OrderBy(lineWithVerse => lineWithVerse.VerseNumber);
        // list.Sort((objOne, objTwo) => objOne.LineNumber - objTwo.LineNumber);

        //!2) Transforming the DTO back to Line object
        var Lines = list.GroupBy(line => line.LineNumber).Select(dto => new Line
        {
            LineNumber = dto.Key,
            LineID = dto.First().LineID,
            Text = dto.First().Text,
            IsCentered = dto.First().IsCentered,
            LineType = dto.First().LineType,
            SurahNumber = dto.First().SurahNumber,
            PageNumber = dto.First().PageNumber,
            VersesKeys = dto.First().VersesKeys,
            Verses = dto.Select(verse => new Verse
            {
                VerseID = verse.VerseID,
                ChapterID = verse.ChapterID,
                HizbNumber = verse.VerseNumber,
                JuzNumber = verse.JuzNumber,
                Page = verse.Page,
                Sajdah = verse.Sajdah,
                TextUthmani = verse.TextUthmani,
                VerseNumber = verse.VerseNumber
            }).ToList()
        });


        return Ok(Lines);
    }
    [HttpGet("/api/home/")]
    public async Task<IActionResult> GetAll()
    {
        List<List<Line>> list = new();
        for (int i = 1; i <= 604; i++)
        {
            var tempList = (await repository.ExecuteStoredProcedure(i, "GetLineWithVerses", "PageNumber")).
            OrderBy(lineWithVerse => lineWithVerse.LineNumber)
            .OrderBy(lineWithVerse => lineWithVerse.VerseNumber);

            var Lines = tempList.GroupBy(line => line.LineNumber).Select(dto => new Line
            {
                LineNumber = dto.Key,
                LineID = dto.First().LineID,
                Text = dto.First().Text,
                IsCentered = dto.First().IsCentered,
                LineType = dto.First().LineType,
                SurahNumber = dto.First().SurahNumber,
                PageNumber = dto.First().PageNumber,
                VersesKeys = dto.First().VersesKeys,
                Verses = dto.Select(verse => new Verse
                {
                    VerseID = verse.VerseID,
                    ChapterID = verse.ChapterID,
                    HizbNumber = verse.VerseNumber,
                    JuzNumber = verse.JuzNumber,
                    Page = verse.Page,
                    Sajdah = verse.Sajdah,
                    TextUthmani = verse.TextUthmani,
                    VerseNumber = verse.VerseNumber
                }).ToList()
            });
            list.Add(Lines.ToList());
        }
        return Ok(list);
    }
}

