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
}
