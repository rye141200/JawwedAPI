using System;
using JawwedAPI.Core.Domain.Entities;

namespace JawwedAPI.Core.DTOs;

public class LineResponse
{
    public int LineNumber { get; set; }

    public string? LineType { get; set; }

    public int SurahNumber { get; set; }

    public int PageNumber { get; set; }

    public string? Text { get; set; }

    public bool IsCentered { get; set; }

    public List<VerseKeyAudio> VersesKeys { get; set; } = [];

    public int? JuzNumber { get; set; }
    public int? HizbNumber { get; set; }
    public int? RubHizbNumber { get; set; }
}

public class VerseKeyAudio
{
    public string VerseKey { get; set; } = string.Empty;
    public List<string> Audio { get; set; } = new List<string>();
}
