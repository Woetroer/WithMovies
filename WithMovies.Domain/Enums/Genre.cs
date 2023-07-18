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
}
