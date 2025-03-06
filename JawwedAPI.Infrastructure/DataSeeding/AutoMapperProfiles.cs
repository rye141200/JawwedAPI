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
            .ForMember(dest => dest.Audios, options => options.MapFrom<BookMarkResponseResolver>());

        CreateMap<Line, LineResponse>()
            .ForMember(
                lineResponse => lineResponse.VersesKeys,
                options => options.MapFrom<LineResponseResolver>()
            );

        CreateMap<Verse, VerseResponse>()
            .ForMember(
                verseResponse => verseResponse.Audios,
                options => options.MapFrom<VerseResponseResolver>()
            )
            .ForMember(
                verseResponse => verseResponse.Text,
                options => options.MapFrom(verse => verse.TextUthmani)
            );

        CreateMap<Chapter, ChapterResponse>()
            .ForMember(
                chapterResponse => chapterResponse.ChapterNumber,
                options => options.MapFrom(chapter => chapter.ChapterID)
            )
            .ForMember(
                ChapterNumber => ChapterNumber.PagesRange,
                options => options.MapFrom(chapter => chapter.Pages)
            );

        CreateMap<JsonChapter, Chapter>();
        CreateMap<JsonLine, Line>();
        CreateMap<JsonVerse, Verse>();
        CreateMap<Mofasir, MofasirResponse>();
        CreateMap<JsonZekr, Zekr>();
    }
}
