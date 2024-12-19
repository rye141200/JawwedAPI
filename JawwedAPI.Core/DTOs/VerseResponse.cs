using System;

namespace JawwedAPI.Core.DTOs;

public class VerseResponse
{
    public int VerseNumber { get; set; }

    public int JuzNumber { get; set; }

    public int HizbNumber { get; set; }

    public int Page { get; set; }

    public bool? Sajdah { get; set; }

    public string? Text { get; set; }

    public int ChapterID { get; set; }

    public List<string> Audios { get; set; } = new List<string>();
}
