using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Helpers;
using JawwedAPI.Core.Options;
using Microsoft.Extensions.Options;

namespace JawwedAPI.Core.Mapper;

public class VerseResponseResolver(IOptions<AudioAssetsOptions> audioOptions)
    : IValueResolver<Verse, VerseResponse, List<string>>
{
    public List<string> Resolve(
        Verse source,
        VerseResponse destination,
        List<string> destMember,
        ResolutionContext context
    )
    {
        string baselink = audioOptions.Value.AudioBaseLink;
        List<string> reciters = audioOptions.Value.Mashaykh;
        List<string> audioLinks = new List<string>();

        foreach (string reciter in reciters)
        {
            string[] reciterParts = reciter.Split('/');

            List<Func<string>> predicates =
            [
                () => baselink,
                .. reciterParts.Select(part => (Func<string>)(() => part)),
                () => "mp3",
            ];

            predicates.Add(() => $"{source.ChapterID:D3}{source.VerseNumber:D3}.mp3");

            var templateSource =
                reciterParts.Length > 1
                    ? audioOptions.Value.LinkExampleMurattal
                    : audioOptions.Value.LinkExampleSimple;

            var template = AssetsGenerator.GenerateTemplate(templateSource);
            var link = AssetsGenerator.GenerateLink(template, predicates);
            audioLinks.Add(link);
        }

        return audioLinks;
    }
}
