namespace JawwedAPI.Core.DTOs;


public class LineWithVerseDTO
{
    public int LineID { get; set; }
    public int LineNumber { get; set; }
    public string LineType { get; set; } = null!;
    public int SurahNumber { get; set; }
    public int PageNumber { get; set; }
    public string Text { get; set; } = null!;
    public bool IsCentered { get; set; }
    public string VersesKeys { get; set; } = null!;
    public int VerseID { get; set; }
    public int VerseNumber { get; set; }
    public int JuzNumber { get; set; }
    public int HizbNumber { get; set; }
    public int Page { get; set; }
    public bool Sajdah { get; set; }
    public string TextUthmani { get; set; } = null!;
    public int ChapterID { get; set; }
}
