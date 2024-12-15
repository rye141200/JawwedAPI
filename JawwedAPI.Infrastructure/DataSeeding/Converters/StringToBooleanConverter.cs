using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JawwedAPI.Infrastructure.DataSeeding.Converters;

public class StringToBooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Expected a string value.");

        string value = reader.GetString();
        return value == "1";
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "1" : "0");
    }
}