using System;
using System.Text.Json.Serialization;

namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

public class JsonTafsir
{
    [JsonPropertyName("chapterVerseId")]
    public required string ChapterVerseID { get; set; }
    [JsonPropertyName("text")]
    public required string Text { get; set; }
    [JsonPropertyName("mofasirId")]
    public int MofasirID { get; set; }
}
