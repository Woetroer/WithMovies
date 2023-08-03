namespace WithMovies.Domain;

public class SearchResults
{
    /// <summary>
    /// Duration the query took to execute in seconds
    /// </summary>
    public required double Time { get; set; }
    public required PreviewDto[] Movies { get; set; }
}
