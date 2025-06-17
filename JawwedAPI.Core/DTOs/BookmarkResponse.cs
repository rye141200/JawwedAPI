using System.Text.Json;
using System.Text.Json.Serialization;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? VerseKey { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual string? Verse { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Page { get; set; }

    public List<string> Audios { get; set; } = [];
    public BookmarkType BookmarkType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Zekr? Zekr { get; set; }
}
