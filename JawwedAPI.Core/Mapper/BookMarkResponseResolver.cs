using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Helpers;
using JawwedAPI.Core.Options;
using Microsoft.Extensions.Options;
namespace JawwedAPI.Core.Mapper;

public class BookMarkResponseResolver(IOptions<AudioAssetsOptions> audioAssetsOptions) : IValueResolver<Bookmark, BookmarkResponse, List<string>>
{

    public List<string> Resolve(Bookmark source, BookmarkResponse destination, List<string> destMember, ResolutionContext context)
    {
        string baselink = audioAssetsOptions.Value.AudioBaseLink;
        List<string> mashaykhList = audioAssetsOptions.Value.Mashaykh;

        List<string> possibleLinks = new List<string>();
        foreach (string shaykh in mashaykhList)
        {
            List<Func<string>> predicates = [() => baselink];
            string[] shaykhParts = shaykh.Split('/');
            foreach (var part in shaykhParts)
            {
                predicates.Add(() => part);
            }
            predicates.Add(() => "mp3");
            predicates.Add(() =>
                {
                    string[] verseXchapter = source.VerseKey.Split(':').Select(param => int.Parse(param).ToString("D3")).ToArray();
                    return $"{verseXchapter[0]}{verseXchapter[1]}.mp3";
                });
            string template = AssetsGenerator.GenerateTemplate(audioAssetsOptions.Value.LinkExampleSimple);
            if (shaykhParts.Length > 1)
                template = AssetsGenerator.GenerateTemplate(audioAssetsOptions.Value.LinkExampleMurattal);

            possibleLinks.Add(AssetsGenerator.GenerateLink<string>(template, predicates));
        }

        return possibleLinks;
    }
}
