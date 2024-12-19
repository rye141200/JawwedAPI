using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Mapper;
namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<BookmarkAddRequest, Bookmark>();
        CreateMap<Bookmark, BookmarkResponse>()
        .ForMember(dest => dest.Audios, options => options.MapFrom<BookMarkResponseResolver>()
        );

        CreateMap<Line, LineResponse>().
        ForMember(lineResponse => lineResponse.JuzNumber,
        options => options.MapFrom(line => Math.Ceiling(line.PageNumber / 20.0) == 31 ? 30 :
        Math.Ceiling(line.PageNumber / 20.0))).
        ForMember(lineResponse => lineResponse.HizbNumber,
        options => options.MapFrom(line => Math.Ceiling(line.PageNumber / 10.0))).
        ForMember(lineResponse => lineResponse.RubHizbNumber,
        options => options.MapFrom(line => Math.Ceiling((line.PageNumber % 10) / 2.5))).
        ForMember(lineResponse => lineResponse.VersesKeys, options => options.MapFrom<LineResponseResolver>());

        CreateMap<Verse, VerseResponse>().
        ForMember(verseResponse => verseResponse.Audios, options => options.MapFrom<VerseResponseResolver>()).
        ForMember(verseResponse => verseResponse.Text, options => options.MapFrom(verse => verse.TextUthmani));

        CreateMap<Chapter,ChapterResponse>().
        ForMember(chapterResponse => chapterResponse.ChapterNumber, options => options.MapFrom(chapter => chapter.ChapterID)).
        ForMember(ChapterNumber => ChapterNumber.PagesRange, options => options.MapFrom(chapter => chapter.Pages));

        CreateMap<JsonChapter, Chapter>();
        CreateMap<JsonLine, Line>();
        CreateMap<JsonVerse, Verse>();
    }
}