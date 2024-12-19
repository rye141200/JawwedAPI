using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Helpers;
using JawwedAPI.Core.Options;
using Microsoft.Extensions.Options;

namespace JawwedAPI.Core.Mapper;

public class LineResponseResolver(IOptions<AudioAssetsOptions> audioOptions) : IValueResolver<Line, LineResponse, List<VerseKeyAudio>>
{
    
    public List<VerseKeyAudio> Resolve(Line source, LineResponse destination, List<VerseKeyAudio> destMember, ResolutionContext context)
    {
        List<VerseKeyAudio> versesKeys = [];
        if (string.IsNullOrEmpty(source.VersesKeys))
            return versesKeys;

        string[]? verses = source.VersesKeys?.Split(',');

        if (verses is null) return versesKeys;
        foreach (var verse in verses)
        {
            string baselink = audioOptions.Value.AudioBaseLink;
            List<string> reciters = audioOptions.Value.Mashaykh;
            List<string> audioLinks = new List<string>();

            foreach (string reciter in reciters)
            {
                string[] reciterParts = reciter.Split('/');

                List<Func<string>> predicates = [() => baselink, .. reciterParts.Select(part => (Func<string>)(() => part)), () => "mp3"];

                string[] verseParts = verse
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
            versesKeys.Add(new VerseKeyAudio { VerseKey = verse, Audio = audioLinks });
        }

        return versesKeys;
    }

}
