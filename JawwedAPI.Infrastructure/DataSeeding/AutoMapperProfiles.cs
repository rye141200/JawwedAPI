using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
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

        CreateMap<Goal, GoalResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ReadingSchedule, opt => opt.MapFrom<GoalResponseResolver>());
        CreateMap<CreateGoalRequest, Goal>();

        CreateMap<JsonQuestion, Question>()
            .ForMember(dest => dest.OptionA, opt => opt.MapFrom(src => src.Options[0]))
            .ForMember(dest => dest.OptionB, opt => opt.MapFrom(src => src.Options[1]));

        CreateMap<JsonChapter, Chapter>();
        CreateMap<JsonLine, Line>();
        CreateMap<JsonVerse, Verse>();
        CreateMap<Mofasir, MofasirResponse>();
        CreateMap<JsonZekr, Zekr>();
    }
}
