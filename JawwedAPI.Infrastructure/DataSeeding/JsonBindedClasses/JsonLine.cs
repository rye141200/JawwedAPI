using System.Text.Json.Serialization;
using JawwedAPI.Infrastructure.DataSeeding.Converters;

namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;

public class JsonLine
{
    [JsonPropertyName("line_number")]
    public int LineNumber { get; set; }

    [JsonPropertyName("line_type")]
    public string? LineType { get; set; }

    [JsonPropertyName("is_centered")]
    [JsonConverter(typeof(IntToBooleanConverter))]
    public bool IsCentered { get; set; }

    [JsonPropertyName("page_number")]
    public int PageNumber { get; set; }

    [JsonPropertyName("surah_number")]
    public int SurahNumber { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("verse_keys")]
    public string? VersesKeys { get; set; }

    [JsonPropertyName("juz_number")]
    public int JuzNumber { get; set; }

    [JsonPropertyName("hizb_number")]
    public int HizbNumber { get; set; }

    [JsonPropertyName("rub_el_hizb_number")]
    public int RubHizbNumber { get; set; }
}
