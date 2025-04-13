using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.DTOs;

public class BookmarkAddRequest
{
    public Guid UserId { get; set; }

    [Required]
    public string VerseKey { get; set; } = null!;

    [Required]
    public string Verse { get; set; } = null!;

    [Required]
    public string Page { get; set; } = null!;
}
