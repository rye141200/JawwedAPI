using System;
using System.Text.Json.Serialization;
using JawwedAPI.Infrastructure.DataSeeding.Converters;

namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

public class JsonZekr
{
    [JsonPropertyName("id")]
    public int ZekrID { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("category_audio")]
    public required string CategoryAudio { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("audio")]
    public required string Audio { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("category_id")]
    public int CategoryId { get; set; }
}
