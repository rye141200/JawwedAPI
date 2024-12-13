
using System.Text.Json.Serialization;
using JawwedAPI.Infrastructure.DataSeeding.Converters;

namespace JawwedAPI.Infrastructure.DataSeeding.JsonBindedClasses;


public class JsonChapter
{
    [JsonPropertyName("id")]
    public int ChapterID { get; set; }

    [JsonPropertyName("revelation_place")]
    public string? RevelationPlace { get; set; }

    [JsonPropertyName("revelation_order")]
    public int RevelationOrder { get; set; }

    [JsonPropertyName("bismillah_pre")]
    public bool BismallahPre { get; set; }

    [JsonPropertyName("name_simple")]
    public string? NameSimple { get; set; }

    [JsonPropertyName("name_complex")]
    public string? NameComplex { get; set; }

    [JsonPropertyName("name_arabic")]
    public string? NameArabic { get; set; }

    [JsonPropertyName("verses_count")]
    public int VersesCount { get; set; }

    [JsonPropertyName("pages")]
    [JsonConverter(typeof(IntArrayToStringConverter))]
    public string? Pages { get; set; }

    [JsonPropertyName("english_name")]
    public string? NameEnglish { get; set; }
}
