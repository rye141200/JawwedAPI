using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Helpers;
using JawwedAPI.Core.Options;
using Microsoft.Extensions.Options;

namespace JawwedAPI.Core.Mapper;

/// <summary>
/// Resolves audio links for bookmarks by generating URLs for different reciters
/// </summary>
public class BookMarkResponseResolver(IOptions<AudioAssetsOptions> audioOptions) : IValueResolver<Bookmark, BookmarkResponse, List<string>>
{
    /// <summary>
    /// Generates a list of audio URLs for a given bookmark based on configured reciters
    /// </summary>
    /// <param name="source">Source bookmark entity containing verse information</param>
    /// <param name="destination">Destination DTO for the bookmark response</param>
    /// <param name="destMember">Destination member collection</param>
    /// <param name="context">AutoMapper resolution context</param>
    /// <returns>List of generated audio URLs for different reciters</returns>
    public List<string> Resolve(Bookmark source, BookmarkResponse destination, List<string> destMember, ResolutionContext context)
    {
        string baselink = audioOptions.Value.AudioBaseLink;
        List<string> reciters = audioOptions.Value.Mashaykh;
        List<string> audioLinks = new List<string>();

        foreach (string reciter in reciters)
        {
            string[] reciterParts = reciter.Split('/');

            List<Func<string>> predicates = [() => baselink, .. reciterParts.Select(part => (Func<string>)(() => part)), () => "mp3"];

            string[] verseParts = source.VerseKey
                .Split(':')
                .Select(part => int.Parse(part).ToString("D3"))
                .ToArray();

            predicates.Add(() => $"{verseParts[0]}{verseParts[1]}.mp3");

            var templateSource = reciterParts.Length > 1
                ? audioOptions.Value.LinkExampleMurattal
                : audioOptions.Value.LinkExampleSimple;

            var template = AssetsGenerator.GenerateTemplate(templateSource);
            var link = AssetsGenerator.GenerateLink(template, predicates);
            audioLinks.Add(link);
        }

        return audioLinks;
    }
}
