using System.ComponentModel.DataAnnotations;
using JawwedAPI.Core.Domain.Enums;

namespace JawwedAPI.Core.DTOs;

public class BookmarkAddRequest
{
    public Guid? UserId { get; set; } = Guid.Empty;

    public string? VerseKey { get; set; }

    public string? Verse { get; set; }

    public string? Page { get; set; }

    public BookmarkType BookmarkType { get; set; }

    public int? ZekrID { get; set; }
}
