namespace JawwedAPI.Core.DTOs;


public class BookmarkAddRequest
{
    public int UserId { get; set; }
    public string VerseKey { get; set; } = null!;
    public string Verse { get; set; } = null!;
    public string Page { get; set; } = null!;

}
