using System;
using System.Text.Json.Serialization;

namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

public class JsonQuestion
{
    [JsonPropertyName("question")]
    public string QuestionHeader { get; set; } = string.Empty;

    [JsonPropertyName("options")]
    public string[] Options { get; set; } = Array.Empty<string>();

    [JsonPropertyName("answer")]
    public string Answer { get; set; } = string.Empty;

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }
}
