using System.Text.Json;
using System.Text.Json.Serialization;

namespace Purchases.Domain.Serializers
{
    public class StringOrIntConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetInt64().ToString(),
                JsonTokenType.Null => null,
                _ => throw new JsonException($"Unsupported token type: {reader.TokenType}")
            };
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            => writer.WriteStringValue(value);

    }
}
