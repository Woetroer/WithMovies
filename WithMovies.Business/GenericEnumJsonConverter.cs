using System.Text.Json;
using System.Text.Json.Serialization;

namespace WithMovies.Business;

public class GenericEnumJsonConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    public override T Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return Enum.Parse<T>(
            reader.GetString()!.Replace(" ", "").Replace("-", "").Replace("&", "And")
        );
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Enum.GetName<T>(value));
    }
}
