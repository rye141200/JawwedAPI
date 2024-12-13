using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Helpers;
using JawwedAPI.Core.Options;
using Microsoft.Extensions.Options;

namespace JawwedAPI.Core.DTOs;
/// <summary>
/// Represents a Data Transfer Object for a bookmark.
/// that will be used in presentation layer
/// </summary>
public class BookmarkResponse
{

    public string? VerseKey { get; set; }
    virtual public string? Verse { get; set; }
    public string? Page { get; set; }
    public List<string> Audios { get; set; } = new List<string>();

}
