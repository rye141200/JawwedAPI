using System;
using System.Text.Json.Serialization;
using JawwedAPI.Infrastructure.DataSeeding.Converters;

namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

public class JsonVerse
{
    [JsonPropertyName("VerseID")]
    public int VerseID { get; set; }

    [JsonPropertyName("VerseNumber")]
    public int VerseNumber { get; set; }

    [JsonPropertyName("JuzNumber")]
    public int JuzNumber { get; set; }

    [JsonPropertyName("HizbNumber")]
    public int HizbNumber { get; set; }

    [JsonPropertyName("Page")]
    public int Page { get; set; }

    [JsonPropertyName("Sajdah")]
    [JsonConverter(typeof(StringToBooleanConverter))]
    public bool? Sajdah { get; set; }

    [JsonPropertyName("TextUthmani")]
    public string? TextUthmani { get; set; }

    //! Foreign => Chapter
    [JsonPropertyName("ChapterID")]
    public int ChapterID { get; set; }
}
