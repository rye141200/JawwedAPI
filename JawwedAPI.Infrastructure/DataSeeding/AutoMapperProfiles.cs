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
        // CreateMap<AppUser, MemberDto>().
        // ForMember(memberDto => memberDto.Age,
        // options => options.MapFrom(appUser => appUser.DateOfBirth.CalculateAge())
        // ).
        // ForMember(memberDto => memberDto.PhotoUrl, options => options.MapFrom(
        //     appUser =>
        //     appUser.Photos.FirstOrDefault(photo => photo.IsMain)!.Url
        //     )
        // );
        // CreateMap<Photo, PhotoDto>();
        CreateMap<BookmarkAddRequest, Bookmark>();
        CreateMap<Bookmark, BookmarkResponse>()
        .ForMember(dest => dest.Audios, options => options.MapFrom<BookMarkResponseResolver>()
        );
        CreateMap<JsonChapter, Chapter>();
        CreateMap<JsonLine, Line>();
    }
}