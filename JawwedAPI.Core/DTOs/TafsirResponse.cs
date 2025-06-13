using System;

namespace JawwedAPI.Core.DTOs;

public class TafsirResponse
{
    public required string ChapterVerseID { get; set; }
    public required string Text { get; set; }
}
