using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JawwedAPI.Infrastructure.DataSeeding.Converters;

public class IntArrayToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var pages = new List<int>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                pages.Add(reader.GetInt32());
            }

            return string.Join(":", pages);
        }

        throw new JsonException("Expected an array for pages.");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        var parts = value.Split(':').Select(int.Parse).ToArray();
        writer.WriteStartArray();

        foreach (var part in parts)
            writer.WriteNumberValue(part);

        writer.WriteEndArray();
    }
}
