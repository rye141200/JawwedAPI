using System;

namespace JawwedAPI.Core.DTOs;

public class ChapterResponse
{
    public int ChapterNumber { get; set; }
    public string? RevelationPlace { get; set; }
    public Boolean BismallahPre { get; set; }
    public string? NameComplex { get; set; }
    public string? NameArabic { get; set; }
    public string? NameEnglish { get; set; }
    public string? PagesRange { get; set; }
    public int VersesCount { get; set; }
    public List<VerseResponse>? Verses { get; set; }
}
