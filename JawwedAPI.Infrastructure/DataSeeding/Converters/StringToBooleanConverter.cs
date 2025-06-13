using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JawwedAPI.Infrastructure.DataSeeding.Converters;

public class StringToBooleanConverter : JsonConverter<bool>
{
    public override bool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        return value.ToLower() == "true" || value == "1";
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
