
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WithMovies.Domain.Enums
{
	public enum MovieStatus
	{
		None,
		Canceled,
		Rumored,
		Planned,
		InProduction,
		PostProduction,
		Released,
	}

    public class MovieStatusJsonConverter : JsonConverter<MovieStatus>
    {
        public override MovieStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Enum.Parse<MovieStatus>(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, MovieStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Enum.GetName<MovieStatus>(value));
        }
    }
}

