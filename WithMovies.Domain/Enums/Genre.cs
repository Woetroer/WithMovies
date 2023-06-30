using System.Text.Json;
using System.Text.Json.Serialization;

namespace WithMovies.Domain.Enums
{
	public enum Genre : int
	{
        Crime = 0,
        Adventure,
        Documentary,
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Western,
        Horror,
        Drama,
        Comedy,
        Action,
        War,
        Music,
        TvMovie,
        History,
        Thriller,
        Animation,
        Family,
        Foreign,
    }

    public class GenreJsonConverter : JsonConverter<Genre>
    {
        public override Genre Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Enum.Parse<Genre>(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Genre value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Enum.GetName<Genre>(value));
        }
    }
}