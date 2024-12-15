using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Helpers;
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
        options => options.MapFrom(line => Math.Ceiling((line.PageNumber % 10) / 2.5)));


        CreateMap<JsonChapter, Chapter>();
        CreateMap<JsonLine, Line>();
        CreateMap<JsonVerse, Verse>();
    }
}