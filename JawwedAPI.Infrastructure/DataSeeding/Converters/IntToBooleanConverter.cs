using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace JawwedAPI.Infrastructure.DataSeeding.Converters;

public class IntToBooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected an array for pages.");

        return reader.GetInt32() == 1;
    }


    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
